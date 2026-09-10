# Changes since forking from Telyl/WOTRMesmerist

This fork (`nub088/WOTRMesmerist`) branched off Telyl's original mod just after
its [3.3 release](https://github.com/Telyl/WOTRMesmerist/releases/tag/3.3).
Everything below is new since that point.

## New archetype: Vexing Daredevil

- Full implementation of the Pathfinder: Occult Adventures mesmerist archetype
  (13 new files under `Mesmerist/Class/Mesmerist/Archetypes/VexingDaredevil/`).
- Martial Weapon Proficiency (1st) reuses the vanilla weapon-proficiency
  selection blueprint directly.
- Dazzling Feint and its 7 sub-options (Blinding Strike, Combat Maneuver,
  Critical Strike, Outmaneuver, Piercing Strike, Sloppy Defense, Surprise
  Strike), plus Improved/Greater/Greater Mesmerizing Feint bonus feats.
- Some effects are approximated where the engine has no clean hook for the
  tabletop trigger (e.g. Dazzling Feint fires off hits against a
  hypnotically-stared target rather than a true "successful feint" check).

## New mesmerist tricks (experimental unless noted)

Fleet in Shadows, Astounding Avoidance, Reflect Fear, Shadow Splinter,
Spectral Smoke, Gift of Will, Umbral Shield, Faked Death, Compel Alacrity,
Levitation Buffer, Misdirection, Cursed Sanction, Vision of Blood.

## New bold stares

Unaided, Allure, Sensed.

Oscillation and Susceptibility were prototyped and then dropped: their tabletop
effects (distance-gated concealment, social skill penalties) have no engine
hook, and the stand-ins just duplicated Disorientation and Infiltration.

## New feats

- **Force of Personality** — Charisma replaces Wisdom as the Will save base
  attribute (stacks with Towering Ego, applying Cha twice).
- **Extra Mesmerist Trick** / **Extra Bold Stare** — repeatable feats gated at
  mesmerist level 1 / 3.

## Custom icons

64 generated icons (SDXL, 128px) wired to every trick, bold stare, and class
feature, with a graceful fallback to the stock icon if a file is missing.

## Bug fixes

- Force of Personality, Extra Mesmerist Trick, and Extra Bold Stare were each
  showing up twice at level-up (redundant/non-idempotent feat-selection
  registration).
- Vexing Daredevil was showing up twice at character creation (the archetype
  was registered on the Mesmerist class both by `ArchetypeConfigurator`
  itself and by a redundant explicit call).
- `CreateMasterfulTrick` referenced the wrong Free in Body blueprint, letting
  masterful tricks stack onto a target already carrying it.

## Other

- Text content cleanup (removed em dashes throughout).
- `EXPERIMENTAL` registration regions added so low-confidence features can be
  stripped out cleanly if needed.
