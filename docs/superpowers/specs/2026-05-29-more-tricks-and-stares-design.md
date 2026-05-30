# Design: More Mesmerist Tricks & Bold Stares

**Date:** 2026-05-29
**Repo:** Telyl/WOTRMesmerist (fork branch `feature/more-tricks-and-stares`)
**Goal:** Add as many additional tabletop Mesmerist tricks and bold stares as are
portable to the *Wrath of the Righteous* engine, staying faithful to the author's
existing design and coding conventions.

---

## 1. Hard constraint (read first)

This mod **cannot be compiled or run in the authoring environment.** `Mesmerist.csproj`
references the installed game's assemblies (`$(WrathInstallDir)\Wrath_Data\Managed\*.dll`:
`Assembly-CSharp.dll`, `Unity*.dll`, `Owlcat*.dll`, `UnityModManager.dll`) plus a
Windows-pathed `TabletopTweaks-Core.dll`, and targets `net481`. None of that exists here.

**Consequence:** All new code is written by pattern-matching the author's existing files.
It is *plausible and convention-correct* but **unverified** — no compiler check, no runtime
check. Each feature below carries a confidence rating. The user builds and tests on their
own machine (see §6). Low-confidence features may need fixups or be dropped at build time.

---

## 2. Faithfulness principles

1. **Finish what the author started first.** He already minted GUIDs (and helper sub-GUIDs)
   for nine tricks and one bold stare he never implemented, and left them in a commented-out
   selection list in `TrickSelection.cs`. Completing these reuses his GUIDs, names, and intent.
2. **Reuse his helpers and patterns verbatim:** `CommonTrickHelpers.CreateTrick` /
   `CreateMasterfulTrick` for tricks; the `Buff` + `Feature` + selection-registration shape for
   bold stares; `AddContextStatBonus` with the standard scaling
   `ContextRankConfigs.CharacterLevel(...).WithCustomProgression((7,2),(19,3),(20,5))`.
3. **Borrow icons** from `AbilityRefs`/`BuffRefs`/`FeatureRefs` like he does (no new art).
4. **Match file layout:** one class per file under `Class/Mesmerist/Tricks/` or
   `Class/Mesmerist/BoldStares/`, namespace and `Configure()` static method per his style.
5. **Skip content with no meaning in a CRPG** rather than fake it (see §5).
6. **Keep Layer 3 strippable.** Layer 3 (and any other Low-confidence) features must be
   isolated so they can be removed in one move if they break the build, without touching
   Layer 1/2 code. Concretely:
   - Each Layer 3 feature lives in its own file, header-commented `// LAYER 3 — experimental`.
   - All Layer 3 `Configure()` calls are grouped under a single
     `// === LAYER 3 (experimental — comment out this block to strip) ===` region in the
     relevant selection files, and their `Guids.<Name>` entries in the `AddToAllFeatures([...])`
     lists are grouped on their own clearly-commented lines.
   - Their GUID constants sit in a dedicated `#region Layer3Experimental` block in `Guids.cs`.
   This mirrors the author's existing habit of commenting out unfinished blocks.

---

## 3. Integration points (every feature touches several of these)

A new **bold stare** requires ALL of:
- `Utils/Guids.cs` — GUID constants (`<Name>`, `<Name>Buff`). *Unaided already has both.*
- `Class/Mesmerist/BoldStares/<Name>.cs` — buff (penalty) + selectable feature.
- `Class/Mesmerist/BoldStares/BoldStare.cs` — add `<Name>.Configure();` and add
  `Guids.<Name>` to the `AddToAllFeatures([...])` list.
- **`Class/Mesmerist/HypnoticStare.cs`** — add a `.Conditional(CasterHasFact(Guids.<Name>),`
  `ifTrue: ApplyBuffPermanent(Guids.<Name>Buff, true, false))` branch into the stare ability's
  action chain. **This is the step that actually applies the stare to the target.**
- `LocalizedStrings.json` — `<Name>.Name` and `<Name>.Description` entries.

A new **trick** requires ALL of:
- `Utils/Guids.cs` — `<Name>`, `<Name>Ability`, `<Name>Buff` (+ any effect GUIDs). *The nine
  scaffolded tricks already have these.*
- `Class/Mesmerist/Tricks/<Name>.cs` — `CommonTrickHelpers.CreateTrick(...)` (1 trick point)
  or `CreateMasterfulTrick(...)` (2 points, auto-adds `MasterfulTricks` prerequisite), followed
  by a `BuffConfigurator.For(<Buff>)...` block adding the actual effect.
- `Class/Mesmerist/Tricks/TrickSelection.cs` — add `<Name>.Configure();` and add `Guids.<Name>`
  to `AddToAllFeatures([...])`.
- **`Utils/CommonTrickHelpers.cs`** — append `Guids.<Name>Buff` to BOTH `AddAbilityTargetHasFact`
  `checkedFacts` arrays (the "don't double-implant a trick on one creature" guard).
- `LocalizedStrings.json` — `<Name>.Name` and `<Name>.Description` entries.

---

## 4. Features to implement

Confidence = likelihood it compiles and behaves correctly without fixups, given the patterns
the author already proved work.

### Layer 1 — Finish the author's scaffolded set (GUIDs already exist)

| Feature | Type | Tabletop effect | Implementation approach | Confidence |
|---|---|---|---|---|
| **Unaided** | Bold stare | Target can't grant or receive flanking. | Stare buff add-on granting a "cannot be flanked / cannot flank" mechanic (mirror the engine's uncanny-dodge "cannot be flanked" fact). Wire into `HypnoticStare.cs` chain. | Medium |
| **Fleet in Shadows** | Trick | Double speed (max +30) in dim/dark, 1 round. | Buff with `AddBuffMovementSpeed` (as Sluggishness does, but positive). Drop the "dim light only" gate — apply unconditionally, note deviation. | High |
| **Astounding Avoidance** | Trick | Evasion vs magical attacks; improved at 12th. | Buff grants Improved Evasion fact. | Medium |
| **Spectral Smoke** | Trick | Illusory fog cloud, radius scales, 1 rnd/level. | Clone game Fog Cloud area-effect (author scaffolded `SpectralSmokeAreaEffect`/`AreaEffectBuff`). | Medium |
| **Reflect Fear** | Trick | Suppress fear 1d4 rnds; source saves or shaken. | Buff removes/immunizes fear for duration; drop the reflect-to-source half (note deviation). | Medium |
| **Shadow Splinter** | Trick | Reduce damage to subject by 3+Cha; redirect. | Buff grants flat untyped damage reduction 3+Cha; drop redirect (note deviation). | Medium |
| **Cursed Sanction** | Masterful | Attacker takes −4 atk/saves/checks 1 min/lvl. | Buff with attacked-trigger applying a −4 debuff to the attacker. | Low-Med |
| **Compel Alacrity** | Trick | 10 ft free movement (+5/5 lvls), no AoO. | Per author's scaffold (`...DimensionDoorAbility`/`Feat`): short no-AoO reposition. | Low |
| **Levitation Buffer** | Trick | Lift/bull-rush adjacent enemies 1 round. | Knockback/forced-movement ability vs adjacent enemies. | Low |
| **Misdirection** | Trick | Mesmerist feints attacker → denies Dex to AC. | Per author's scaffold (`MisdirectionFeintFeat`): grant feint vs the triggering attacker. | Low |

### Layer 2 — Bold stares that are pure stare-penalty extensions (new GUIDs)

These mirror Disorientation/Timidity exactly — add `AddContextStatBonus` with the standard
scaling onto a stare buff, plus a `.Conditional` branch in `HypnoticStare.cs`.

| Feature | Tabletop effect | Implementation | Confidence |
|---|---|---|---|
| **Allure** | Penalty also on initiative + Perception. | Stat penalty on Initiative + Perception. | High |
| **Sensed** | Penalty also on Stealth. | Stat penalty on Stealth. | High |
| **Oscillation** | Target treats enemies >30 ft as concealed. | Best-effort: grant target a miss-chance debuff; distance gate likely not expressible. | Low (may drop) |
| **Susceptibility** | Penalty on Sense Motive; harder vs Diplomacy/Intimidate on target. | Penalty on Perception/Persuasion analogue; mostly social. | Low (may drop) |

### Layer 3 — Other combat-meaningful tricks (new GUIDs)

| Feature | Type | Tabletop effect | Implementation | Confidence |
|---|---|---|---|---|
| **Gift of Will** | Trick | Subject uses mesmerist's Will save / +Cha to social. | Competence bonus to Will saves (approximate; note deviation). | Medium |
| **Umbral Shield** | Trick | Ignore bright-light penalties; immune to dazzled. | Buff with condition-immunity to Dazzled. | Medium |
| **Vision of Blood** | Masterful | Attacker of subject saves or stunned 1 rnd. | Attacked-trigger → save-or-stun debuff on attacker. | Low-Med |
| **Faked Death** | Masterful | Subject appears dead + invisible. | Buff grants Invisibility; drop the "appears dead" figment. | Medium |

---

## 5. Deliberately skipped (with reason)

- **Social / exploration-only tricks** (no WOTR mechanic): Enchanting Words, Life Revier,
  Psychic Impression, Telepathic Link, Chain of Eyes, Willful Ignorance, Break Stupor,
  Mental Fallback, Spell Anticipation, Spatial Switch, Avian Escape, Umbral Transformation,
  Reflection of Weakness (ability damage is rare), Allay Pain (nonlethal is rare).
- **Mask Misery / Greater Mask Misery:** redundant with the already-implemented Touch Treatment.
- **Stares not expressible in-engine:** Restriction (dim light → difficult terrain),
  Nightblindness (reduce darkvision *range*).
- **Devilbane gazes** (Occult Origins): anti-outsider / planar-binding niche; near-zero value
  in normal play.

---

## 6. Build & verification (user, on a machine with WOTR installed)

1. `git checkout feature/more-tricks-and-stares`
2. Ensure WOTR + UnityModManager + TabletopTweaks-Core are installed; fix the hardcoded
   `TabletopTweaks-Core` `HintPath` in `Mesmerist.csproj` to the local path (pre-existing issue).
3. `dotnet build Mesmerist.sln -c Release` (auto-detects the game path via `GamePath.props`).
4. Fix any compile errors surfaced by the unverified features (expect some in Low-confidence rows).
5. Launch the game, create/level a Mesmerist, confirm each new trick appears in the trick
   selection and each new stare in the bold-stare selection, and that effects apply on use.

---

## 7. Risk summary

- **Low risk:** Layer 1 simple buffs (Fleet in Shadows), Layer 2 High-confidence stares
  (Allure, Sensed), Unaided — all reuse proven patterns.
- **Medium risk:** features needing facts/components used elsewhere in-game but not yet in this
  mod (Improved Evasion, area effects, condition immunity, invisibility).
- **High risk / may be dropped:** reactive "when subject is attacked" triggers (Cursed Sanction,
  Vision of Blood), feint (Misdirection), forced movement (Levitation Buffer, Compel Alacrity),
  distance-gated concealment (Oscillation). These will be best-effort; if the required component
  can't be confirmed from the BlueprintCore/game API, the feature is stubbed with a TODO and
  flagged for the user rather than guessed wildly.
- **Stripping experimental features — two methods (verified against the built code):**
  - **Deactivate (safe, keeps compiling — recommended):** comment out the
    `=== EXPERIMENTAL ... ===` `Configure()` block AND the grouped `// EXPERIMENTAL` lines in the
    `AddToAllFeatures([...])` list, in BOTH `TrickSelection.cs` and `BoldStare.cs`. That removes the
    features from the game. Everything still compiles: the per-feature files, GUIDs, the
    `CommonTrickHelpers.cs` exclusion-array entries, and the `HypnoticStare.cs` dispatch branches
    for Oscillation/Susceptibility all remain harmless (their `CasterHasFact` conditions simply
    never match because the features are no longer granted).
  - **Full delete (more cleanup):** to also delete the per-feature `.cs` files and the
    `#region Layer3Experimental` GUIDs, you MUST additionally remove (a) the matching
    `Guids.<Name>Buff` entries from both `checkedFacts` arrays in `CommonTrickHelpers.cs`, and
    (b) for the two experimental stares, their `.Conditional(...)` branches in `HypnoticStare.cs`.
    Otherwise those files reference deleted GUID constants and won't compile. Note: the
    Compel Alacrity / Levitation Buffer / Misdirection / Cursed Sanction GUIDs live in the main
    GUID region (the author pre-allocated them), not in `#region Layer3Experimental`.
  - Layer 1/2 core features are unaffected either way.
