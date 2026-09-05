#!/usr/bin/env python
"""Generate ability icons for the Mesmerist mod with SDXL on ROCm.

    python gen_icons.py --pilot                 # the 8 pilot icons
    python gen_icons.py --all                   # everything defined in icon_prompts
    python gen_icons.py --only HypnoticStare Nightmare
    python gen_icons.py --only Nightmare --seed 12345   # reroll one that missed

Masters are written full size to tools/icon_masters/ so an icon can be re-cropped or
re-exported without paying for generation again; the game-ready downscales land in
Mesmerist/Assets/Icons/, which is where IconLoader reads them from.
"""
import argparse
import hashlib
import os
import pathlib
import sys

# The card also drives the desktop, so the ~2GB the compositor holds is not ours to use.
# Expandable segments would let the allocator hand fragmented blocks back rather than
# stranding them in the reserve pool. ROCm logs "expandable_segments not supported on this
# platform" and ignores it - harmless to leave set, but it is not what fixed the decode OOM;
# see the VAE tiling note below. Must precede torch initialising its allocator.
os.environ.setdefault("PYTORCH_ALLOC_CONF", "expandable_segments:True")

import torch
from diffusers import StableDiffusionXLPipeline
from PIL import Image

import icon_prompts

REPO = pathlib.Path(__file__).resolve().parent.parent
MASTERS = REPO / "tools" / "icon_masters"
SHIPPED = REPO / "Mesmerist" / "Assets" / "Icons"

MODEL = "stabilityai/stable-diffusion-xl-base-1.0"
GEN_SIZE = 1024
ICON_SIZE = 128


def stable_seed(name: str) -> int:
    """Deterministic per-icon seed, so a rerun reproduces the same art.

    Digest the name rather than reducing its bytes: int.from_bytes(...) % 2**31 keeps only
    the last four characters or so, which collided every icon ending in "Stare" onto one
    seed - seven of them, all eye-themed prompts that would then have rendered near-alike.
    """
    return int.from_bytes(hashlib.sha256(name.encode()).digest()[:4], "big") % (2**31)


def build_pipeline() -> StableDiffusionXLPipeline:
    if not torch.cuda.is_available():
        sys.exit("No GPU visible to torch. Check the ROCm install before generating.")
    print(f"device: {torch.cuda.get_device_name(0)}")

    pipe = StableDiffusionXLPipeline.from_pretrained(
        MODEL, torch_dtype=torch.float16, variant="fp16", use_safetensors=True
    ).to("cuda")
    # The decode, not the UNet, is what runs this card out of memory: SDXL's VAE is upcast
    # to fp32 (force_upcast - it overflows in fp16), so one conv at full resolution asks for
    # 4.5GB in a single allocation. Slicing splits across the batch and so does nothing at
    # batch size 1; tiling is what splits a single image.
    #
    # enable_tiling() alone is not enough. tiled_decode only kicks in when the latent is
    # strictly larger than tile_latent_min_size, which AutoencoderKL derives from the VAE's
    # configured sample_size - 1024 for SDXL, giving a threshold of exactly 128. A 1024px
    # generation makes a 128 latent, and 128 > 128 is false, so the guard silently falls
    # through to the untiled path. Lowering the threshold is what actually engages tiling.
    pipe.vae.enable_slicing()
    pipe.vae.enable_tiling()
    pipe.vae.tile_sample_min_size = 512
    pipe.vae.tile_latent_min_size = 64
    if not pipe.vae.use_tiling:
        sys.exit("VAE tiling did not stick; the 1024px decode will not fit in VRAM.")
    pipe.set_progress_bar_config(disable=True)
    return pipe


def generate(pipe, name: str, subject: str, seed: int | None, steps: int) -> None:
    seed = stable_seed(name) if seed is None else seed
    prompt = f"{subject}, {icon_prompts.STYLE}"
    generator = torch.Generator(device="cuda").manual_seed(seed)

    image = pipe(
        prompt=prompt,
        negative_prompt=icon_prompts.NEGATIVE,
        width=GEN_SIZE,
        height=GEN_SIZE,
        num_inference_steps=steps,
        guidance_scale=7.0,
        generator=generator,
    ).images[0]

    MASTERS.mkdir(parents=True, exist_ok=True)
    SHIPPED.mkdir(parents=True, exist_ok=True)

    master_path = MASTERS / f"{name}.png"
    image.save(master_path)

    icon = image.resize((ICON_SIZE, ICON_SIZE), Image.LANCZOS)
    icon.save(SHIPPED / f"{name}.png")

    print(f"  {name:22} seed={seed:<12} -> {master_path.name} + {ICON_SIZE}px")


def main() -> None:
    ap = argparse.ArgumentParser()
    group = ap.add_mutually_exclusive_group(required=True)
    group.add_argument("--pilot", action="store_true", help="the pilot batch")
    group.add_argument("--all", action="store_true", help="every defined icon")
    group.add_argument("--only", nargs="+", metavar="NAME", help="specific icons")
    ap.add_argument("--seed", type=int, default=None,
                    help="override the deterministic seed (only useful with --only)")
    ap.add_argument("--steps", type=int, default=32)
    ap.add_argument("--skip-existing", action="store_true",
                    help="leave icons that already have a master alone; regenerate a "
                         "particular one by deleting its master first")
    args = ap.parse_args()

    if args.pilot:
        targets = icon_prompts.PILOT
    elif args.all:
        targets = icon_prompts.ALL
    else:
        missing = [n for n in args.only if n not in icon_prompts.ALL]
        if missing:
            sys.exit(f"No prompt defined for: {', '.join(missing)}")
        targets = {n: icon_prompts.ALL[n] for n in args.only}

    if args.seed is not None and len(targets) > 1:
        sys.exit("--seed applies to a single icon; narrow it with --only.")

    if args.skip_existing:
        # Keyed off the shipped icon, not the master: the shipped PNG is the deliverable, and
        # an approved icon can outlive its master (PainfulStare's was dropped deliberately).
        # Keying off the master would silently regenerate and overwrite art already chosen.
        kept = [n for n in targets
                if (SHIPPED / f"{n}.png").exists() or (MASTERS / f"{n}.png").exists()]
        targets = {n: v for n, v in targets.items() if n not in kept}
        if kept:
            print(f"keeping {len(kept)} already-generated: {', '.join(sorted(kept))}")

    print(f"generating {len(targets)} icon(s) at {GEN_SIZE}px, {args.steps} steps")
    pipe = build_pipeline()
    for name, subject in targets.items():
        generate(pipe, name, subject, args.seed, args.steps)
    print(f"\nmasters:  {MASTERS}\nshipped:  {SHIPPED}")


if __name__ == "__main__":
    main()
