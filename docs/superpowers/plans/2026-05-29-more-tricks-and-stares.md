# More Mesmerist Tricks & Bold Stares — Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Add the portable tabletop Mesmerist tricks and bold stares the mod is missing, faithful to the author's existing patterns, with all low-confidence/Layer-3 content isolated for easy stripping.

**Architecture:** Each *trick* = `CommonTrickHelpers.CreateTrick`/`CreateMasterfulTrick` + a `BuffConfigurator.For(...)` effect, registered in `TrickSelection.cs` and added to the two exclusion arrays in `CommonTrickHelpers.cs`. Each *bold stare* = a penalty `Buff` + a selectable `Feature` (mirroring `Disorientation.cs`), registered in `BoldStare.cs`, with a `.Conditional` branch added to the hardcoded dispatch in `HypnoticStare.cs`. All feature text lives in `LocalizedStrings.json`; all GUIDs in `Utils/Guids.cs`.

**Tech Stack:** C# (net481), WW-Blueprint-Core, TabletopTweaks-Core, UnityModManager. Targets Pathfinder: WOTR.

---

## ⚠️ Verification reality (read before executing)

This project **cannot be compiled or run in the authoring environment** (no game DLLs). Therefore:
- There is **no automated test framework**. The TDD "write failing test → pass" loop does not apply.
- Per-task verification = **structural checks** (grep) that every integration point is wired and GUIDs are consistent. These are runnable here and are the honest analog of tests.
- **Real verification (compile + in-game) is deferred to the user** on a machine with WOTR (see Task 20).
- For **low-confidence** features, prefer a working stub + a clearly-marked `// TODO(verify-on-build):` over a wild guess. Do not invent APIs; if unsure, use the candidate shown and flag it.

## Confidence buckets → where each feature is registered

- **CORE** (registered normally): Fleet in Shadows, Astounding Avoidance, Reflect Fear, Shadow Splinter, Spectral Smoke (L1 tricks); Unaided (L1 stare); Allure, Sensed (L2 stares).
- **EXPERIMENTAL** (registered inside a single comment-out region; strip if they break the build): Compel Alacrity, Levitation Buffer, Misdirection, Cursed Sanction (L1 low/reactive); Oscillation, Susceptibility (L2 low); Gift of Will, Umbral Shield, Vision of Blood, Faked Death (L3).

---

## File Structure

**Created** (one class per file):
- `Class/Mesmerist/Tricks/FleetInShadows.cs`, `AstoundingAvoidance.cs`, `ReflectFear.cs`, `ShadowSplinter.cs`, `SpectralSmoke.cs`, `CursedSanction.cs`, `CompelAlacrity.cs`, `LevitationBuffer.cs`, `Misdirection.cs`, `GiftOfWill.cs`, `UmbralShield.cs`, `VisionOfBlood.cs`, `FakedDeath.cs`
- `Class/Mesmerist/BoldStares/Unaided.cs`, `Allure.cs`, `Sensed.cs`, `Oscillation.cs`, `Susceptibility.cs`

**Modified:**
- `Utils/Guids.cs` — add L2 GUIDs + a `#region Layer3Experimental` block.
- `Class/Mesmerist/Tricks/TrickSelection.cs` — `Configure()` calls + `AddToAllFeatures`, with an EXPERIMENTAL region.
- `Class/Mesmerist/BoldStares/BoldStare.cs` — `Configure()` calls + `AddToAllFeatures`, with an EXPERIMENTAL region.
- `Class/Mesmerist/HypnoticStare.cs` — one `.Conditional` branch per new stare.
- `Utils/CommonTrickHelpers.cs` — append each new trick buff to BOTH `checkedFacts` arrays.
- `LocalizedStrings.json` — Name/Description per feature.

> **Layer-1 tricks already have GUIDs** (`AstoundingAvoidance*`, `CompelAlacrity*`, `CursedSanction*`, `FleetInShadows*`, `LevitationBuffer*`, `Misdirection*`, `ReflectFear*`, `ShadowSplinter*`, `SpectralSmoke*`) and **Unaided** has `Unaided`/`UnaidedBuff`. Do not re-create those.

---

## Task 1: Add new GUIDs (Layer 2 stares + Layer 3 region)

**Files:** Modify `Utils/Guids.cs`

- [ ] **Step 1: Add Layer 2 stare GUIDs** after the existing bold-stare block (after the `ManifoldStare15th` line, before `#endregion` of the Mesmerist region):

```csharp
        // --- Layer 2 bold stares (added) ---
        internal const string Allure = "a104069a4138466ea64b34b15b37a0cb";
        internal const string AllureBuff = "c22d0d27c3fc4cd0ac933f363cfe6ed0";
        internal const string Sensed = "8053a176b89b46dcb6463e8d87ad4c61";
        internal const string SensedBuff = "0bdf94fed89a41f5a170246ab7ad71c7";
        internal const string Oscillation = "63070ed584b74c8d8c39cc8b0ddb356a";
        internal const string OscillationBuff = "28be789e15c042fe8b34382691bfcd5f";
        internal const string Susceptibility = "9d4a5740fcbd459091ee48f6c1169895";
        internal const string SusceptibilityBuff = "a3d86c8f9e8448a29e52fd3cdb00a2c2";
```

- [ ] **Step 2: Add a Layer-3 region** immediately before the final `}` that closes the `Guids` class (after the `#region Homebrew` block):

```csharp
        #region Layer3Experimental
        // Experimental Layer-3 tricks. Strip by deleting this region + the matching
        // EXPERIMENTAL blocks in TrickSelection.cs and CommonTrickHelpers.cs.
        internal const string GiftOfWill = "445a19b78a464d4fbbf96a239df464b9";
        internal const string GiftOfWillAbility = "4e19c89dedcb48fc976dbc48faf07422";
        internal const string GiftOfWillBuff = "ba50cfc619624b7581c6d2a177227bd8";
        internal const string UmbralShield = "7691f24adf244513bf8d65adf900d5d0";
        internal const string UmbralShieldAbility = "9adfbbfd102649f08c060f563e3fb851";
        internal const string UmbralShieldBuff = "bc34345188994b078ae902f4f557f57a";
        internal const string VisionOfBlood = "b9787690000e4882a85e403aea5b0656";
        internal const string VisionOfBloodAbility = "887cec662acd48aeb76d20ace8a47abd";
        internal const string VisionOfBloodBuff = "6a9e41f582564819b32cec4d0e460027";
        internal const string VisionOfBloodDebuff = "823d81191249438a97c69fc5b1e0c718";
        internal const string FakedDeath = "84b5b43f326d41e882999dc27cb8edaf";
        internal const string FakedDeathAbility = "ee50cbbc01be436baef99de6a5367440";
        internal const string FakedDeathBuff = "2e28fe2316b44bddb381448beef0d53a";
        internal const string FakedDeathInvisBuff = "c2d34986c5ca4bdbafc25e464c18bafc";
        #endregion
```

- [ ] **Step 3: Verify no duplicate GUIDs**

Run: `cd Mesmerist && grep -oE '"[0-9a-f]{32}"' Utils/Guids.cs | sort | uniq -d`
Expected: no output (empty = all unique).

- [ ] **Step 4: Commit**

```bash
git add Mesmerist/Utils/Guids.cs
git commit -m "feat(guids): add Layer 2 stare and Layer 3 experimental trick GUIDs"
```

---

## Task 2: Fleet in Shadows (trick, CORE)

Tabletop (Occult Realms): double speed (max +30 ft) in dim/dark light for 1 round. **Deviation:** applied unconditionally as a flat +30 ft (the engine can't gate cheaply on light level); standard min/level trick duration.

**Files:** Create `Class/Mesmerist/Tricks/FleetInShadows.cs`; Modify `TrickSelection.cs`, `CommonTrickHelpers.cs`, `LocalizedStrings.json`

- [ ] **Step 1: Create the trick file**

```csharp
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils.Types;
using Kingmaker.Enums;
using Mesmerist.Utils;

namespace Mesmerist.Class.Mesmerist.Tricks
{
    internal class FleetInShadows
    {
        public static void Configure()
        {
            CommonTrickHelpers.CreateTrick("FleetInShadows",
                                           "FleetInShadows.Name",
                                           "FleetInShadows.Description",
                                           AbilityRefs.Grace.Reference.Get().Icon,
                                           Guids.FleetInShadows,
                                           Guids.FleetInShadowsAbility,
                                           Guids.FleetInShadowsBuff);

            BuffConfigurator.For(Guids.FleetInShadowsBuff)
                .AddBuffMovementSpeed(descriptor: ModifierDescriptor.UntypedStackable, value: 30, cappedMinimum: false, minimumCap: 0)
                .Configure();
        }
    }
}
```

- [ ] **Step 2: Register in `TrickSelection.cs`** — add to the `Configure()` body (with the other `.Configure()` calls) and to `AddToAllFeatures`:

Add call: `FleetInShadows.Configure();`
Add to `AddToAllFeatures([...])`: `Guids.FleetInShadows,`

- [ ] **Step 3: Add buff to BOTH exclusion arrays in `CommonTrickHelpers.cs`** — append `Guids.FleetInShadowsBuff` to the `checkedFacts: [...]` list in `CreateTrick` AND in `CreateMasterfulTrick`.

- [ ] **Step 4: Add localization to `LocalizedStrings.json`** (insert two objects into the top-level array):

```json
    { "Key": "FleetInShadows.Name", "ProcessTemplates": false, "enGB": "Fleet in Shadows" },
    { "Key": "FleetInShadows.Description", "enGB": "The subject's base speed increases by 30 feet for 1 minute per mesmerist level." },
```

- [ ] **Step 5: Structural verify**

Run:
```bash
cd Mesmerist
grep -c "FleetInShadows.Configure();" Class/Mesmerist/Tricks/TrickSelection.cs   # 1
grep -c "Guids.FleetInShadows\b" Class/Mesmerist/Tricks/TrickSelection.cs        # >=1
grep -c "Guids.FleetInShadowsBuff" Utils/CommonTrickHelpers.cs                   # 2
grep -c "FleetInShadows.Name\|FleetInShadows.Description" LocalizedStrings.json  # 2
```
Expected counts as noted.

- [ ] **Step 6: Commit**

```bash
git add Mesmerist/Class/Mesmerist/Tricks/FleetInShadows.cs Mesmerist/Class/Mesmerist/Tricks/TrickSelection.cs Mesmerist/Utils/CommonTrickHelpers.cs Mesmerist/LocalizedStrings.json
git commit -m "feat(trick): add Fleet in Shadows"
```

---

## Task 3: Astounding Avoidance (trick, CORE)

Tabletop: evasion vs magical attacks (improved at 12th). **Implementation:** grant the Improved Evasion fact for the buff duration. **Deviation:** applies to all Reflex-half effects (engine evasion), not only "magical."

**Files:** Create `Class/Mesmerist/Tricks/AstoundingAvoidance.cs`; Modify `TrickSelection.cs`, `CommonTrickHelpers.cs`, `LocalizedStrings.json`

- [ ] **Step 1: Create the trick file**

```csharp
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils.Types;
using Mesmerist.Utils;

namespace Mesmerist.Class.Mesmerist.Tricks
{
    internal class AstoundingAvoidance
    {
        public static void Configure()
        {
            CommonTrickHelpers.CreateTrick("AstoundingAvoidance",
                                           "AstoundingAvoidance.Name",
                                           "AstoundingAvoidance.Description",
                                           AbilityRefs.Displacement.Reference.Get().Icon,
                                           Guids.AstoundingAvoidance,
                                           Guids.AstoundingAvoidanceAbility,
                                           Guids.AstoundingAvoidanceBuff);

            // CANDIDATE: FeatureRefs.ImprovedEvasion. If that ref name doesn't resolve,
            // fall back to FeatureRefs.Evasion. // TODO(verify-on-build)
            BuffConfigurator.For(Guids.AstoundingAvoidanceBuff)
                .AddFacts(new() { FeatureRefs.ImprovedEvasion.Reference.Get() })
                .Configure();
        }
    }
}
```

- [ ] **Step 2: Register** in `TrickSelection.cs`: add `AstoundingAvoidance.Configure();` and `Guids.AstoundingAvoidance,` to `AddToAllFeatures`.
- [ ] **Step 3: Exclusion arrays** — append `Guids.AstoundingAvoidanceBuff` to BOTH `checkedFacts` arrays in `CommonTrickHelpers.cs`.
- [ ] **Step 4: Localization**

```json
    { "Key": "AstoundingAvoidance.Name", "ProcessTemplates": false, "enGB": "Astounding Avoidance" },
    { "Key": "AstoundingAvoidance.Description", "enGB": "For 1 minute per mesmerist level, the subject gains improved evasion: it takes no damage on a successful Reflex save against an effect that allows a save for half, and half damage on a failed save." },
```

- [ ] **Step 5: Structural verify**

```bash
cd Mesmerist
grep -c "AstoundingAvoidance.Configure();" Class/Mesmerist/Tricks/TrickSelection.cs   # 1
grep -c "Guids.AstoundingAvoidanceBuff" Utils/CommonTrickHelpers.cs                   # 2
grep -c "AstoundingAvoidance.Name\|AstoundingAvoidance.Description" LocalizedStrings.json  # 2
```

- [ ] **Step 6: Commit**

```bash
git add Mesmerist/Class/Mesmerist/Tricks/AstoundingAvoidance.cs Mesmerist/Class/Mesmerist/Tricks/TrickSelection.cs Mesmerist/Utils/CommonTrickHelpers.cs Mesmerist/LocalizedStrings.json
git commit -m "feat(trick): add Astounding Avoidance"
```

---

## Task 4: Reflect Fear (trick, CORE)

Tabletop: suppress a fear effect for 1d4 rounds; source saves or shaken. **Implementation:** buff grants immunity to the Fear descriptor for the duration. **Deviation:** drops the "reflect shaken back to source" half.

**Files:** Create `Class/Mesmerist/Tricks/ReflectFear.cs`; Modify `TrickSelection.cs`, `CommonTrickHelpers.cs`, `LocalizedStrings.json`

- [ ] **Step 1: Create the trick file**

```csharp
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints.Classes.Spells;
using Mesmerist.Utils;

namespace Mesmerist.Class.Mesmerist.Tricks
{
    internal class ReflectFear
    {
        public static void Configure()
        {
            CommonTrickHelpers.CreateTrick("ReflectFear",
                                           "ReflectFear.Name",
                                           "ReflectFear.Description",
                                           AbilityRefs.Eyebite.Reference.Get().Icon,
                                           Guids.ReflectFear,
                                           Guids.ReflectFearAbility,
                                           Guids.ReflectFearBuff);

            // CANDIDATE: AddBuffDescriptorImmunity. If unavailable, fall back to
            // .AddFacts(new() { BuffRefs.RemoveFearBuff.Reference.Get() }). // TODO(verify-on-build)
            BuffConfigurator.For(Guids.ReflectFearBuff)
                .AddBuffDescriptorImmunity(descriptor: SpellDescriptor.Fear)
                .Configure();
        }
    }
}
```

- [ ] **Step 2: Register** in `TrickSelection.cs`: add `ReflectFear.Configure();` and `Guids.ReflectFear,`.
- [ ] **Step 3: Exclusion arrays** — append `Guids.ReflectFearBuff` to BOTH arrays in `CommonTrickHelpers.cs`.
- [ ] **Step 4: Localization**

```json
    { "Key": "ReflectFear.Name", "ProcessTemplates": false, "enGB": "Reflect Fear" },
    { "Key": "ReflectFear.Description", "enGB": "For 1 minute per mesmerist level, the subject is immune to fear effects." },
```

- [ ] **Step 5: Structural verify**

```bash
cd Mesmerist
grep -c "ReflectFear.Configure();" Class/Mesmerist/Tricks/TrickSelection.cs   # 1
grep -c "Guids.ReflectFearBuff" Utils/CommonTrickHelpers.cs                   # 2
grep -c "ReflectFear.Name\|ReflectFear.Description" LocalizedStrings.json     # 2
```

- [ ] **Step 6: Commit**

```bash
git add Mesmerist/Class/Mesmerist/Tricks/ReflectFear.cs Mesmerist/Class/Mesmerist/Tricks/TrickSelection.cs Mesmerist/Utils/CommonTrickHelpers.cs Mesmerist/LocalizedStrings.json
git commit -m "feat(trick): add Reflect Fear"
```

---

## Task 5: Shadow Splinter (trick, CORE)

Tabletop: reduce damage to subject by 3 + Cha; redirect to a nearby creature. **Implementation:** flat physical damage reduction of 3. **Deviation:** physical only, flat 3 (no Cha scaling), no redirect.

**Files:** Create `Class/Mesmerist/Tricks/ShadowSplinter.cs`; Modify `TrickSelection.cs`, `CommonTrickHelpers.cs`, `LocalizedStrings.json`

- [ ] **Step 1: Create the trick file**

```csharp
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils.Types;
using Mesmerist.Utils;

namespace Mesmerist.Class.Mesmerist.Tricks
{
    internal class ShadowSplinter
    {
        public static void Configure()
        {
            CommonTrickHelpers.CreateTrick("ShadowSplinter",
                                           "ShadowSplinter.Name",
                                           "ShadowSplinter.Description",
                                           AbilityRefs.Displacement.Reference.Get().Icon,
                                           Guids.ShadowSplinter,
                                           Guids.ShadowSplinterAbility,
                                           Guids.ShadowSplinterBuff);

            // CANDIDATE: AddDamageResistancePhysical. // TODO(verify-on-build)
            BuffConfigurator.For(Guids.ShadowSplinterBuff)
                .AddDamageResistancePhysical(value: 3)
                .Configure();
        }
    }
}
```

- [ ] **Step 2: Register** in `TrickSelection.cs`: add `ShadowSplinter.Configure();` and `Guids.ShadowSplinter,`.
- [ ] **Step 3: Exclusion arrays** — append `Guids.ShadowSplinterBuff` to BOTH arrays.
- [ ] **Step 4: Localization**

```json
    { "Key": "ShadowSplinter.Name", "ProcessTemplates": false, "enGB": "Shadow Splinter" },
    { "Key": "ShadowSplinter.Description", "enGB": "For 1 minute per mesmerist level, the subject gains DR 3/— against physical damage as illusory shadows absorb part of each blow." },
```

- [ ] **Step 5: Structural verify**

```bash
cd Mesmerist
grep -c "ShadowSplinter.Configure();" Class/Mesmerist/Tricks/TrickSelection.cs   # 1
grep -c "Guids.ShadowSplinterBuff" Utils/CommonTrickHelpers.cs                   # 2
grep -c "ShadowSplinter.Name\|ShadowSplinter.Description" LocalizedStrings.json  # 2
```

- [ ] **Step 6: Commit**

```bash
git add Mesmerist/Class/Mesmerist/Tricks/ShadowSplinter.cs Mesmerist/Class/Mesmerist/Tricks/TrickSelection.cs Mesmerist/Utils/CommonTrickHelpers.cs Mesmerist/LocalizedStrings.json
git commit -m "feat(trick): add Shadow Splinter"
```

---

## Task 6: Spectral Smoke (trick, CORE)

Tabletop: illusory fog cloud (radius scales), 1 round/level. **Implementation:** the subject gains partial concealment (mirrors `ShadowBlend.cs`'s `AddConcealment`). **Deviation:** modeled as a personal concealment buff, not a placed area-effect fog (true `AbilityAreaEffect` cloning is out of scope/high-risk; flagged for the user).

**Files:** Create `Class/Mesmerist/Tricks/SpectralSmoke.cs`; Modify `TrickSelection.cs`, `CommonTrickHelpers.cs`, `LocalizedStrings.json`

- [ ] **Step 1: Create the trick file**

```csharp
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils.Types;
using Kingmaker.Enums;
using Mesmerist.Utils;

namespace Mesmerist.Class.Mesmerist.Tricks
{
    internal class SpectralSmoke
    {
        public static void Configure()
        {
            CommonTrickHelpers.CreateTrick("SpectralSmoke",
                                           "SpectralSmoke.Name",
                                           "SpectralSmoke.Description",
                                           AbilityRefs.Echolocation.Reference.Get().Icon,
                                           Guids.SpectralSmoke,
                                           Guids.SpectralSmokeAbility,
                                           Guids.SpectralSmokeBuff);

            BuffConfigurator.For(Guids.SpectralSmokeBuff)
                .AddConcealment(false, false, Concealment.Partial, ConcealmentDescriptor.Fog)
                .Configure();
        }
    }
}
```

- [ ] **Step 2: Register** in `TrickSelection.cs`: add `SpectralSmoke.Configure();` and `Guids.SpectralSmoke,`.
- [ ] **Step 3: Exclusion arrays** — append `Guids.SpectralSmokeBuff` to BOTH arrays.
- [ ] **Step 4: Localization**

```json
    { "Key": "SpectralSmoke.Name", "ProcessTemplates": false, "enGB": "Spectral Smoke" },
    { "Key": "SpectralSmoke.Description", "enGB": "For 1 minute per mesmerist level, illusory smoke wreaths the subject, granting concealment (20% miss chance)." },
```

- [ ] **Step 5: Structural verify**

```bash
cd Mesmerist
grep -c "SpectralSmoke.Configure();" Class/Mesmerist/Tricks/TrickSelection.cs   # 1
grep -c "Guids.SpectralSmokeBuff" Utils/CommonTrickHelpers.cs                   # 2
grep -c "SpectralSmoke.Name\|SpectralSmoke.Description" LocalizedStrings.json   # 2
```

- [ ] **Step 6: Commit**

```bash
git add Mesmerist/Class/Mesmerist/Tricks/SpectralSmoke.cs Mesmerist/Class/Mesmerist/Tricks/TrickSelection.cs Mesmerist/Utils/CommonTrickHelpers.cs Mesmerist/LocalizedStrings.json
git commit -m "feat(trick): add Spectral Smoke (concealment approximation)"
```

---

## Task 7: Unaided (bold stare, CORE)

Tabletop: the stare's target can't grant or receive flanking bonuses.

**Files:** Create `Class/Mesmerist/BoldStares/Unaided.cs`; Modify `BoldStare.cs`, `HypnoticStare.cs`, `LocalizedStrings.json`

- [ ] **Step 1: Create the stare file** (mirrors `Disorientation.cs`; the flanking effect is the candidate component):

```csharp
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using Mesmerist.Utils;

namespace Mesmerist.Class.Mesmerist.BoldStares
{
    public class Unaided
    {
        private static readonly string FeatName = "Unaided";
        internal const string DisplayName = "Unaided.Name";
        private static readonly string Description = "Unaided.Description";

        public static void Configure()
        {
            // CANDIDATE: AddCannotBeFlanked covers "cannot receive flanking". The
            // "cannot grant flanking" direction may need a custom component; if the
            // builder lacks AddCannotBeFlanked, fall back to AddFacts of the game's
            // "cannot be flanked" feature. // TODO(verify-on-build)
            BuffConfigurator.New(FeatName + "Buff", Guids.UnaidedBuff)
                .SetDisplayName(DisplayName)
                .SetDescription(Description)
                .AddUniqueBuff()
                .SetIcon(BuffRefs.DebilitatingInjuryDisorientedEffectBuff.Reference.Get().Icon)
                .AddCannotBeFlanked()
                .Configure();

            FeatureConfigurator.New(FeatName, Guids.Unaided)
                .SetDisplayName(DisplayName)
                .SetDescription(Description)
                .SetIcon(AbilityRefs.TrueSeeing.Reference.Get().Icon)
                .SetIsClassFeature()
                .Configure();
        }
    }
}
```

- [ ] **Step 2: Register in `BoldStare.cs`** — add `Unaided.Configure();` to the body and add `Guids.Unaided` to the `AddToAllFeatures([...])` list.

- [ ] **Step 3: Wire dispatch in `HypnoticStare.cs`** — add this branch into the stare ability's action chain, immediately before `.ApplyBuffPermanent(hypnoticStareBuff, true)`:

```csharp
                   .Conditional(
                               ConditionsBuilder.New().CasterHasFact(Guids.Unaided),
                               ifTrue: ActionsBuilder.New().ApplyBuffPermanent(Guids.UnaidedBuff, true, false))
```

- [ ] **Step 4: Localization**

```json
    { "Key": "Unaided.Name", "ProcessTemplates": false, "enGB": "Unaided" },
    { "Key": "Unaided.Description", "enGB": "The target of the hypnotic stare cannot grant or receive flanking bonuses." },
```

- [ ] **Step 5: Structural verify**

```bash
cd Mesmerist
grep -c "Unaided.Configure();" Class/Mesmerist/BoldStares/BoldStare.cs   # 1
grep -c "Guids.Unaided\b" Class/Mesmerist/BoldStares/BoldStare.cs        # >=1
grep -c "Guids.Unaided)" Class/Mesmerist/HypnoticStare.cs               # 1 (the CasterHasFact branch)
grep -c "Unaided.Name\|Unaided.Description" LocalizedStrings.json        # 2
```

- [ ] **Step 6: Commit**

```bash
git add Mesmerist/Class/Mesmerist/BoldStares/Unaided.cs Mesmerist/Class/Mesmerist/BoldStares/BoldStare.cs Mesmerist/Class/Mesmerist/HypnoticStare.cs Mesmerist/LocalizedStrings.json
git commit -m "feat(stare): add Unaided bold stare"
```

---

## Task 8: Allure (bold stare, CORE)

Tabletop: stare penalty also applies to initiative and Perception.

**Files:** Create `Class/Mesmerist/BoldStares/Allure.cs`; Modify `BoldStare.cs`, `HypnoticStare.cs`, `LocalizedStrings.json`

- [ ] **Step 1: Create the stare file** (two stat penalties, copying the `Disorientation.cs` shape):

```csharp
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Mesmerist.Utils;

namespace Mesmerist.Class.Mesmerist.BoldStares
{
    public class Allure
    {
        private static readonly string FeatName = "Allure";
        internal const string DisplayName = "Allure.Name";
        private static readonly string Description = "Allure.Description";

        public static void Configure()
        {
            BuffConfigurator.New(FeatName + "Buff", Guids.AllureBuff)
                .SetDisplayName(DisplayName)
                .SetDescription(Description)
                .AddUniqueBuff()
                .SetIcon(BuffRefs.DebilitatingInjuryDisorientedEffectBuff.Reference.Get().Icon)
                .AddContextStatBonus(StatType.Initiative, ContextValues.Rank(), ModifierDescriptor.UntypedStackable, 2, -1)
                .AddContextStatBonus(StatType.SkillPerception, ContextValues.Rank(), ModifierDescriptor.UntypedStackable, 2, -1)
                .AddContextRankConfig(ContextRankConfigs.CharacterLevel(AbilityRankType.Default).WithCustomProgression((7, 2), (19, 3), (20, 5)))
                .Configure();

            FeatureConfigurator.New(FeatName, Guids.Allure)
                .SetDisplayName(DisplayName)
                .SetDescription(Description)
                .SetIcon(AbilityRefs.TrueSeeing.Reference.Get().Icon)
                .SetIsClassFeature()
                .Configure();
        }
    }
}
```

- [ ] **Step 2: Register in `BoldStare.cs`** — add `Allure.Configure();` and `Guids.Allure` to `AddToAllFeatures`.
- [ ] **Step 3: Wire dispatch in `HypnoticStare.cs`** — add before `.ApplyBuffPermanent(hypnoticStareBuff, true)`:

```csharp
                   .Conditional(
                               ConditionsBuilder.New().CasterHasFact(Guids.Allure),
                               ifTrue: ActionsBuilder.New().ApplyBuffPermanent(Guids.AllureBuff, true, false))
```

- [ ] **Step 4: Localization**

```json
    { "Key": "Allure.Name", "ProcessTemplates": false, "enGB": "Allure" },
    { "Key": "Allure.Description", "enGB": "The hypnotic stare penalty also applies to the target's initiative and Perception checks." },
```

- [ ] **Step 5: Structural verify**

```bash
cd Mesmerist
grep -c "Allure.Configure();" Class/Mesmerist/BoldStares/BoldStare.cs   # 1
grep -c "Guids.Allure)" Class/Mesmerist/HypnoticStare.cs               # 1
grep -c "Allure.Name\|Allure.Description" LocalizedStrings.json         # 2
```

- [ ] **Step 6: Commit**

```bash
git add Mesmerist/Class/Mesmerist/BoldStares/Allure.cs Mesmerist/Class/Mesmerist/BoldStares/BoldStare.cs Mesmerist/Class/Mesmerist/HypnoticStare.cs Mesmerist/LocalizedStrings.json
git commit -m "feat(stare): add Allure bold stare"
```

---

## Task 9: Sensed (bold stare, CORE)

Tabletop: stare penalty also applies to Stealth.

**Files:** Create `Class/Mesmerist/BoldStares/Sensed.cs`; Modify `BoldStare.cs`, `HypnoticStare.cs`, `LocalizedStrings.json`

- [ ] **Step 1: Create the stare file**

```csharp
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Mesmerist.Utils;

namespace Mesmerist.Class.Mesmerist.BoldStares
{
    public class Sensed
    {
        private static readonly string FeatName = "Sensed";
        internal const string DisplayName = "Sensed.Name";
        private static readonly string Description = "Sensed.Description";

        public static void Configure()
        {
            BuffConfigurator.New(FeatName + "Buff", Guids.SensedBuff)
                .SetDisplayName(DisplayName)
                .SetDescription(Description)
                .AddUniqueBuff()
                .SetIcon(BuffRefs.DebilitatingInjuryDisorientedEffectBuff.Reference.Get().Icon)
                .AddContextStatBonus(StatType.SkillStealth, ContextValues.Rank(), ModifierDescriptor.UntypedStackable, 2, -1)
                .AddContextRankConfig(ContextRankConfigs.CharacterLevel(AbilityRankType.Default).WithCustomProgression((7, 2), (19, 3), (20, 5)))
                .Configure();

            FeatureConfigurator.New(FeatName, Guids.Sensed)
                .SetDisplayName(DisplayName)
                .SetDescription(Description)
                .SetIcon(AbilityRefs.TrueSeeing.Reference.Get().Icon)
                .SetIsClassFeature()
                .Configure();
        }
    }
}
```

- [ ] **Step 2: Register in `BoldStare.cs`** — add `Sensed.Configure();` and `Guids.Sensed`.
- [ ] **Step 3: Wire dispatch in `HypnoticStare.cs`** — add before `.ApplyBuffPermanent(hypnoticStareBuff, true)`:

```csharp
                   .Conditional(
                               ConditionsBuilder.New().CasterHasFact(Guids.Sensed),
                               ifTrue: ActionsBuilder.New().ApplyBuffPermanent(Guids.SensedBuff, true, false))
```

- [ ] **Step 4: Localization**

```json
    { "Key": "Sensed.Name", "ProcessTemplates": false, "enGB": "Sensed" },
    { "Key": "Sensed.Description", "enGB": "The hypnotic stare penalty also applies to the target's Stealth checks." },
```

- [ ] **Step 5: Structural verify**

```bash
cd Mesmerist
grep -c "Sensed.Configure();" Class/Mesmerist/BoldStares/BoldStare.cs   # 1
grep -c "Guids.Sensed)" Class/Mesmerist/HypnoticStare.cs               # 1
grep -c "Sensed.Name\|Sensed.Description" LocalizedStrings.json         # 2
```

- [ ] **Step 6: Commit**

```bash
git add Mesmerist/Class/Mesmerist/BoldStares/Sensed.cs Mesmerist/Class/Mesmerist/BoldStares/BoldStare.cs Mesmerist/Class/Mesmerist/HypnoticStare.cs Mesmerist/LocalizedStrings.json
git commit -m "feat(stare): add Sensed bold stare"
```

---

## Task 10: Create the EXPERIMENTAL registration regions

Before adding experimental features, carve the comment-out regions so later tasks slot into them.

**Files:** Modify `Class/Mesmerist/Tricks/TrickSelection.cs`, `Class/Mesmerist/BoldStares/BoldStare.cs`

- [ ] **Step 1: In `TrickSelection.cs`**, add a marked region. After the last CORE `.Configure();` call add:

```csharp
            // === EXPERIMENTAL (low-confidence / Layer 3 — comment out this block to strip) ===
            CompelAlacrity.Configure();
            LevitationBuffer.Configure();
            Misdirection.Configure();
            CursedSanction.Configure();
            GiftOfWill.Configure();
            UmbralShield.Configure();
            VisionOfBlood.Configure();
            FakedDeath.Configure();
            // === END EXPERIMENTAL ===
```

And in `AddToAllFeatures([...])`, add on their own clearly-commented lines at the end of the list:

```csharp
                // EXPERIMENTAL (strip with the block above):
                Guids.CompelAlacrity, Guids.LevitationBuffer, Guids.Misdirection, Guids.CursedSanction,
                Guids.GiftOfWill, Guids.UmbralShield, Guids.VisionOfBlood, Guids.FakedDeath
```

- [ ] **Step 2: In `BoldStare.cs`**, add a marked region for the low-confidence stares. After the last CORE `.Configure();`:

```csharp
            // === EXPERIMENTAL (low-confidence — comment out this block to strip) ===
            Oscillation.Configure();
            Susceptibility.Configure();
            // === END EXPERIMENTAL ===
```

And in `AddToAllFeatures([...])`:

```csharp
                // EXPERIMENTAL (strip with the block above):
                Guids.Oscillation, Guids.Susceptibility
```

> NOTE: These reference classes/GUIDs created in Tasks 11–18. The project won't compile until those exist — that's expected; experimental tasks follow immediately.

- [ ] **Step 3: Commit**

```bash
git add Mesmerist/Class/Mesmerist/Tricks/TrickSelection.cs Mesmerist/Class/Mesmerist/BoldStares/BoldStare.cs
git commit -m "chore: add EXPERIMENTAL registration regions for low-confidence features"
```

---

## Task 11: Oscillation (bold stare, EXPERIMENTAL)

Tabletop: target treats enemies beyond 30 ft as having concealment. **Implementation:** stub — distance-gated concealment isn't cheaply expressible. Create the stare with a flat to-hit penalty as a loose stand-in and flag it.

**Files:** Create `Class/Mesmerist/BoldStares/Oscillation.cs`; Modify `HypnoticStare.cs`, `LocalizedStrings.json` (registration already added in Task 10)

- [ ] **Step 1: Create the stare file** (stand-in: an attack-roll penalty representing the target's difficulty hitting distant foes):

```csharp
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Mesmerist.Utils;

namespace Mesmerist.Class.Mesmerist.BoldStares
{
    public class Oscillation
    {
        private static readonly string FeatName = "Oscillation";
        internal const string DisplayName = "Oscillation.Name";
        private static readonly string Description = "Oscillation.Description";

        public static void Configure()
        {
            // TODO(verify-on-build): true effect is "enemies beyond 30 ft are concealed to
            // the target". Distance-gated concealment needs a custom component. Stand-in below
            // is a flat -2 attack penalty. Drop this stare if the real effect can't be built.
            BuffConfigurator.New(FeatName + "Buff", Guids.OscillationBuff)
                .SetDisplayName(DisplayName)
                .SetDescription(Description)
                .AddUniqueBuff()
                .SetIcon(BuffRefs.DebilitatingInjuryDisorientedEffectBuff.Reference.Get().Icon)
                .AddContextStatBonus(StatType.AdditionalAttackBonus, ContextValues.Rank(), ModifierDescriptor.UntypedStackable, 2, -1)
                .AddContextRankConfig(ContextRankConfigs.CharacterLevel(AbilityRankType.Default).WithCustomProgression((7, 2), (19, 3), (20, 5)))
                .Configure();

            FeatureConfigurator.New(FeatName, Guids.Oscillation)
                .SetDisplayName(DisplayName)
                .SetDescription(Description)
                .SetIcon(AbilityRefs.TrueSeeing.Reference.Get().Icon)
                .SetIsClassFeature()
                .Configure();
        }
    }
}
```

- [ ] **Step 2: Wire dispatch in `HypnoticStare.cs`** — add before `.ApplyBuffPermanent(hypnoticStareBuff, true)`:

```csharp
                   .Conditional(
                               ConditionsBuilder.New().CasterHasFact(Guids.Oscillation),
                               ifTrue: ActionsBuilder.New().ApplyBuffPermanent(Guids.OscillationBuff, true, false))
```

- [ ] **Step 3: Localization**

```json
    { "Key": "Oscillation.Name", "ProcessTemplates": false, "enGB": "Oscillation" },
    { "Key": "Oscillation.Description", "enGB": "The target of the hypnotic stare has trouble focusing on distant foes (approximated as an attack-roll penalty)." },
```

- [ ] **Step 4: Structural verify**

```bash
cd Mesmerist
grep -c "Guids.Oscillation)" Class/Mesmerist/HypnoticStare.cs           # 1
grep -c "Oscillation.Name\|Oscillation.Description" LocalizedStrings.json # 2
```

- [ ] **Step 5: Commit**

```bash
git add Mesmerist/Class/Mesmerist/BoldStares/Oscillation.cs Mesmerist/Class/Mesmerist/HypnoticStare.cs Mesmerist/LocalizedStrings.json
git commit -m "feat(stare): add Oscillation bold stare (experimental stand-in)"
```

---

## Task 12: Susceptibility (bold stare, EXPERIMENTAL)

Tabletop: penalty to Sense Motive (and harder vs Diplomacy/Intimidate on target). **Implementation:** penalty to Perception as the closest WOTR analog; mostly social, low value.

**Files:** Create `Class/Mesmerist/BoldStares/Susceptibility.cs`; Modify `HypnoticStare.cs`, `LocalizedStrings.json`

- [ ] **Step 1: Create the stare file**

```csharp
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Mesmerist.Utils;

namespace Mesmerist.Class.Mesmerist.BoldStares
{
    public class Susceptibility
    {
        private static readonly string FeatName = "Susceptibility";
        internal const string DisplayName = "Susceptibility.Name";
        private static readonly string Description = "Susceptibility.Description";

        public static void Configure()
        {
            // TODO(verify-on-build): tabletop targets Sense Motive + social DCs, which have
            // no real WOTR combat meaning. Stand-in: Perception penalty. Drop if undesired.
            BuffConfigurator.New(FeatName + "Buff", Guids.SusceptibilityBuff)
                .SetDisplayName(DisplayName)
                .SetDescription(Description)
                .AddUniqueBuff()
                .SetIcon(BuffRefs.DebilitatingInjuryDisorientedEffectBuff.Reference.Get().Icon)
                .AddContextStatBonus(StatType.SkillPerception, ContextValues.Rank(), ModifierDescriptor.UntypedStackable, 2, -1)
                .AddContextRankConfig(ContextRankConfigs.CharacterLevel(AbilityRankType.Default).WithCustomProgression((7, 2), (19, 3), (20, 5)))
                .Configure();

            FeatureConfigurator.New(FeatName, Guids.Susceptibility)
                .SetDisplayName(DisplayName)
                .SetDescription(Description)
                .SetIcon(AbilityRefs.TrueSeeing.Reference.Get().Icon)
                .SetIsClassFeature()
                .Configure();
        }
    }
}
```

- [ ] **Step 2: Wire dispatch in `HypnoticStare.cs`** — add before `.ApplyBuffPermanent(hypnoticStareBuff, true)`:

```csharp
                   .Conditional(
                               ConditionsBuilder.New().CasterHasFact(Guids.Susceptibility),
                               ifTrue: ActionsBuilder.New().ApplyBuffPermanent(Guids.SusceptibilityBuff, true, false))
```

- [ ] **Step 3: Localization**

```json
    { "Key": "Susceptibility.Name", "ProcessTemplates": false, "enGB": "Susceptibility" },
    { "Key": "Susceptibility.Description", "enGB": "The target of the hypnotic stare is more easily read and influenced (approximated as a Perception penalty)." },
```

- [ ] **Step 4: Structural verify**

```bash
cd Mesmerist
grep -c "Guids.Susceptibility)" Class/Mesmerist/HypnoticStare.cs               # 1
grep -c "Susceptibility.Name\|Susceptibility.Description" LocalizedStrings.json # 2
```

- [ ] **Step 5: Commit**

```bash
git add Mesmerist/Class/Mesmerist/BoldStares/Susceptibility.cs Mesmerist/Class/Mesmerist/HypnoticStare.cs Mesmerist/LocalizedStrings.json
git commit -m "feat(stare): add Susceptibility bold stare (experimental stand-in)"
```

---

## Task 13: Gift of Will (trick, EXPERIMENTAL / Layer 3)

Tabletop: subject uses the mesmerist's Will save bonus (or +Cha to social). **Implementation:** competence bonus to Will saves scaling with mesmerist level. **Deviation:** flat scaling bonus, not literal "use the mesmerist's save."

**Files:** Create `Class/Mesmerist/Tricks/GiftOfWill.cs`; Modify `CommonTrickHelpers.cs`, `LocalizedStrings.json` (registration in Task 10)

- [ ] **Step 1: Create the trick file**

```csharp
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils.Types;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Mesmerist.Utils;

namespace Mesmerist.Class.Mesmerist.Tricks
{
    // LAYER 3 — experimental
    internal class GiftOfWill
    {
        public static void Configure()
        {
            CommonTrickHelpers.CreateTrick("GiftOfWill",
                                           "GiftOfWill.Name",
                                           "GiftOfWill.Description",
                                           AbilityRefs.GoodHope.Reference.Get().Icon,
                                           Guids.GiftOfWill,
                                           Guids.GiftOfWillAbility,
                                           Guids.GiftOfWillBuff);

            BuffConfigurator.For(Guids.GiftOfWillBuff)
                .AddContextStatBonus(StatType.SaveWill, ContextValues.Rank(), ModifierDescriptor.Competence)
                .AddContextRankConfig(ContextRankConfigs.ClassLevel([Guids.Mesmerist], type: AbilityRankType.Default).WithCustomProgression((4, 2), (9, 3), (14, 4), (19, 5), (20, 6)))
                .Configure();
        }
    }
}
```

- [ ] **Step 2: Exclusion arrays** — append `Guids.GiftOfWillBuff` to BOTH `checkedFacts` arrays in `CommonTrickHelpers.cs`.
- [ ] **Step 3: Localization**

```json
    { "Key": "GiftOfWill.Name", "ProcessTemplates": false, "enGB": "Gift of Will" },
    { "Key": "GiftOfWill.Description", "enGB": "For 1 minute per mesmerist level, the subject gains a competence bonus on Will saving throws (scaling with mesmerist level)." },
```

- [ ] **Step 4: Structural verify**

```bash
cd Mesmerist
grep -c "Guids.GiftOfWillBuff" Utils/CommonTrickHelpers.cs              # 2
grep -c "GiftOfWill.Name\|GiftOfWill.Description" LocalizedStrings.json # 2
```

- [ ] **Step 5: Commit**

```bash
git add Mesmerist/Class/Mesmerist/Tricks/GiftOfWill.cs Mesmerist/Utils/CommonTrickHelpers.cs Mesmerist/LocalizedStrings.json
git commit -m "feat(trick): add Gift of Will (experimental)"
```

---

## Task 14: Umbral Shield (trick, EXPERIMENTAL / Layer 3)

Tabletop: ignore harmful bright light; immune to dazzled. **Implementation:** condition immunity to Dazzled.

**Files:** Create `Class/Mesmerist/Tricks/UmbralShield.cs`; Modify `CommonTrickHelpers.cs`, `LocalizedStrings.json`

- [ ] **Step 1: Create the trick file**

```csharp
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils.Types;
using Kingmaker.UnitLogic;
using Mesmerist.Utils;

namespace Mesmerist.Class.Mesmerist.Tricks
{
    // LAYER 3 — experimental
    internal class UmbralShield
    {
        public static void Configure()
        {
            CommonTrickHelpers.CreateTrick("UmbralShield",
                                           "UmbralShield.Name",
                                           "UmbralShield.Description",
                                           AbilityRefs.Displacement.Reference.Get().Icon,
                                           Guids.UmbralShield,
                                           Guids.UmbralShieldAbility,
                                           Guids.UmbralShieldBuff);

            // CANDIDATE: AddConditionImmunity(UnitCondition.Dazzled). If the builder name
            // differs, fall back to AddBuffStatusCondition / a condition-immunity component.
            // TODO(verify-on-build)
            BuffConfigurator.For(Guids.UmbralShieldBuff)
                .AddConditionImmunity(UnitCondition.Dazzled)
                .Configure();
        }
    }
}
```

- [ ] **Step 2: Exclusion arrays** — append `Guids.UmbralShieldBuff` to BOTH arrays.
- [ ] **Step 3: Localization**

```json
    { "Key": "UmbralShield.Name", "ProcessTemplates": false, "enGB": "Umbral Shield" },
    { "Key": "UmbralShield.Description", "enGB": "For 1 minute per mesmerist level, the subject is immune to the dazzled condition and ignores penalties from bright light." },
```

- [ ] **Step 4: Structural verify**

```bash
cd Mesmerist
grep -c "Guids.UmbralShieldBuff" Utils/CommonTrickHelpers.cs                # 2
grep -c "UmbralShield.Name\|UmbralShield.Description" LocalizedStrings.json  # 2
```

- [ ] **Step 5: Commit**

```bash
git add Mesmerist/Class/Mesmerist/Tricks/UmbralShield.cs Mesmerist/Utils/CommonTrickHelpers.cs Mesmerist/LocalizedStrings.json
git commit -m "feat(trick): add Umbral Shield (experimental)"
```

---

## Task 15: Faked Death (masterful trick, EXPERIMENTAL / Layer 3)

Tabletop: subject appears dead and becomes invisible. **Implementation:** grant invisibility for the duration (mirrors `FreeInBody.cs`'s `AddFacts` of a game buff). **Deviation:** drops the "appears dead" figment.

**Files:** Create `Class/Mesmerist/Tricks/FakedDeath.cs`; Modify `CommonTrickHelpers.cs`, `LocalizedStrings.json`

- [ ] **Step 1: Create the trick file**

```csharp
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils.Types;
using Mesmerist.Utils;

namespace Mesmerist.Class.Mesmerist.Tricks
{
    // LAYER 3 — experimental
    internal class FakedDeath
    {
        public static void Configure()
        {
            CommonTrickHelpers.CreateMasterfulTrick("FakedDeath",
                                                    "FakedDeath.Name",
                                                    "FakedDeath.Description",
                                                    AbilityRefs.Displacement.Reference.Get().Icon,
                                                    Guids.FakedDeath,
                                                    Guids.FakedDeathAbility,
                                                    Guids.FakedDeathBuff);

            // CANDIDATE: BuffRefs.InvisibilityBuff. If the ref name differs, use the
            // game's invisibility buff blueprint. // TODO(verify-on-build)
            BuffConfigurator.For(Guids.FakedDeathBuff)
                .AddFacts(new() { BuffRefs.InvisibilityBuff.Reference.Get() })
                .Configure();
        }
    }
}
```

- [ ] **Step 2: Exclusion arrays** — append `Guids.FakedDeathBuff` to BOTH arrays.
- [ ] **Step 3: Localization**

```json
    { "Key": "FakedDeath.Name", "ProcessTemplates": false, "enGB": "Faked Death" },
    { "Key": "FakedDeath.Description", "enGB": "For 1 minute per mesmerist level, the subject becomes invisible, appearing to have fallen." },
```

- [ ] **Step 4: Structural verify**

```bash
cd Mesmerist
grep -c "Guids.FakedDeathBuff" Utils/CommonTrickHelpers.cs              # 2
grep -c "FakedDeath.Name\|FakedDeath.Description" LocalizedStrings.json # 2
```

- [ ] **Step 5: Commit**

```bash
git add Mesmerist/Class/Mesmerist/Tricks/FakedDeath.cs Mesmerist/Utils/CommonTrickHelpers.cs Mesmerist/LocalizedStrings.json
git commit -m "feat(trick): add Faked Death (experimental)"
```

---

## Task 16: Compel Alacrity (trick, EXPERIMENTAL — stub)

Tabletop: free movement (no AoO). **Implementation:** stub — true "ignore AoO movement" needs custom work (the author scaffolded a dimension-door approach). Ship a small speed buff stand-in + a prominent TODO.

**Files:** Create `Class/Mesmerist/Tricks/CompelAlacrity.cs`; Modify `CommonTrickHelpers.cs`, `LocalizedStrings.json`

- [ ] **Step 1: Create the trick file**

```csharp
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils.Types;
using Kingmaker.Enums;
using Mesmerist.Utils;

namespace Mesmerist.Class.Mesmerist.Tricks
{
    // LAYER 3 — experimental (STUB)
    internal class CompelAlacrity
    {
        public static void Configure()
        {
            CommonTrickHelpers.CreateTrick("CompelAlacrity",
                                           "CompelAlacrity.Name",
                                           "CompelAlacrity.Description",
                                           AbilityRefs.Grace.Reference.Get().Icon,
                                           Guids.CompelAlacrity,
                                           Guids.CompelAlacrityAbility,
                                           Guids.CompelAlacrityBuff);

            // TODO(verify-on-build): real effect = burst of movement that provokes no AoO.
            // Stand-in: +10 ft speed. Replace with the scaffolded dimension-door approach
            // (Guids.CompelAlacrityDimensionDoorAbility/Feat) or drop this trick.
            BuffConfigurator.For(Guids.CompelAlacrityBuff)
                .AddBuffMovementSpeed(descriptor: ModifierDescriptor.UntypedStackable, value: 10, cappedMinimum: false, minimumCap: 0)
                .Configure();
        }
    }
}
```

- [ ] **Step 2: Exclusion arrays** — append `Guids.CompelAlacrityBuff` to BOTH arrays.
- [ ] **Step 3: Localization**

```json
    { "Key": "CompelAlacrity.Name", "ProcessTemplates": false, "enGB": "Compel Alacrity" },
    { "Key": "CompelAlacrity.Description", "enGB": "The subject moves with supernatural quickness (stand-in: +10 ft speed for 1 minute per mesmerist level)." },
```

- [ ] **Step 4: Structural verify**

```bash
cd Mesmerist
grep -c "Guids.CompelAlacrityBuff" Utils/CommonTrickHelpers.cs                  # 2
grep -c "CompelAlacrity.Name\|CompelAlacrity.Description" LocalizedStrings.json # 2
```

- [ ] **Step 5: Commit**

```bash
git add Mesmerist/Class/Mesmerist/Tricks/CompelAlacrity.cs Mesmerist/Utils/CommonTrickHelpers.cs Mesmerist/LocalizedStrings.json
git commit -m "feat(trick): add Compel Alacrity (experimental stub)"
```

---

## Task 17: Levitation Buffer (trick, EXPERIMENTAL — stub)

Tabletop: lift/bull-rush adjacent enemies. Offensive forced movement — not cheaply expressible as an implanted buff. Ship as a defined-but-inert feature + prominent TODO so it appears in the selection without faking mechanics.

**Files:** Create `Class/Mesmerist/Tricks/LevitationBuffer.cs`; Modify `CommonTrickHelpers.cs`, `LocalizedStrings.json`

- [ ] **Step 1: Create the trick file**

```csharp
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils.Types;
using Mesmerist.Utils;

namespace Mesmerist.Class.Mesmerist.Tricks
{
    // LAYER 3 — experimental (STUB: no functional effect yet)
    internal class LevitationBuffer
    {
        public static void Configure()
        {
            CommonTrickHelpers.CreateTrick("LevitationBuffer",
                                           "LevitationBuffer.Name",
                                           "LevitationBuffer.Description",
                                           AbilityRefs.Grace.Reference.Get().Icon,
                                           Guids.LevitationBuffer,
                                           Guids.LevitationBufferAbility,
                                           Guids.LevitationBufferBuff);

            // TODO(verify-on-build): real effect lifts/bull-rushes adjacent enemies. This
            // requires a custom on-apply forced-movement action. Buff is currently inert.
            // Implement or drop before release.
            BuffConfigurator.For(Guids.LevitationBufferBuff)
                .Configure();
        }
    }
}
```

- [ ] **Step 2: Exclusion arrays** — append `Guids.LevitationBufferBuff` to BOTH arrays.
- [ ] **Step 3: Localization**

```json
    { "Key": "LevitationBuffer.Name", "ProcessTemplates": false, "enGB": "Levitation Buffer" },
    { "Key": "LevitationBuffer.Description", "enGB": "The subject briefly lifts nearby foes off the ground. (Effect not yet implemented — experimental.)" },
```

- [ ] **Step 4: Structural verify**

```bash
cd Mesmerist
grep -c "Guids.LevitationBufferBuff" Utils/CommonTrickHelpers.cs                    # 2
grep -c "LevitationBuffer.Name\|LevitationBuffer.Description" LocalizedStrings.json # 2
```

- [ ] **Step 5: Commit**

```bash
git add Mesmerist/Class/Mesmerist/Tricks/LevitationBuffer.cs Mesmerist/Utils/CommonTrickHelpers.cs Mesmerist/LocalizedStrings.json
git commit -m "feat(trick): add Levitation Buffer (experimental stub)"
```

---

## Task 18: Misdirection (trick, EXPERIMENTAL — stub)

Tabletop: mesmerist feints the attacker, denying its Dex to AC. Feint-on-trigger needs custom work. Ship as defined-but-inert + TODO.

**Files:** Create `Class/Mesmerist/Tricks/Misdirection.cs`; Modify `CommonTrickHelpers.cs`, `LocalizedStrings.json`

- [ ] **Step 1: Create the trick file**

```csharp
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils.Types;
using Mesmerist.Utils;

namespace Mesmerist.Class.Mesmerist.Tricks
{
    // LAYER 3 — experimental (STUB: no functional effect yet)
    internal class Misdirection
    {
        public static void Configure()
        {
            CommonTrickHelpers.CreateTrick("Misdirection",
                                           "Misdirection.Name",
                                           "Misdirection.Description",
                                           AbilityRefs.Displacement.Reference.Get().Icon,
                                           Guids.Misdirection,
                                           Guids.MisdirectionAbility,
                                           Guids.MisdirectionBuff);

            // TODO(verify-on-build): real effect feints the attacker (denies Dex to AC).
            // Needs a trigger + feint action (scaffolded Guids.MisdirectionFeintFeat).
            // Buff is currently inert. Implement or drop before release.
            BuffConfigurator.For(Guids.MisdirectionBuff)
                .Configure();
        }
    }
}
```

- [ ] **Step 2: Exclusion arrays** — append `Guids.MisdirectionBuff` to BOTH arrays.
- [ ] **Step 3: Localization**

```json
    { "Key": "Misdirection.Name", "ProcessTemplates": false, "enGB": "Misdirection" },
    { "Key": "Misdirection.Description", "enGB": "The mesmerist misdirects an attacker, leaving it open. (Effect not yet implemented — experimental.)" },
```

- [ ] **Step 4: Structural verify**

```bash
cd Mesmerist
grep -c "Guids.MisdirectionBuff" Utils/CommonTrickHelpers.cs              # 2
grep -c "Misdirection.Name\|Misdirection.Description" LocalizedStrings.json # 2
```

- [ ] **Step 5: Commit**

```bash
git add Mesmerist/Class/Mesmerist/Tricks/Misdirection.cs Mesmerist/Utils/CommonTrickHelpers.cs Mesmerist/LocalizedStrings.json
git commit -m "feat(trick): add Misdirection (experimental stub)"
```

---

## Task 19: Cursed Sanction (masterful trick, EXPERIMENTAL — reactive)

Tabletop: when the subject is attacked, the attacker takes −4 on attacks/saves/checks for 1 min/level. **Implementation:** define the −4 debuff buff concretely; the reactive "apply on being attacked" trigger is a TODO (needs a `AddTargetAttackedByWeaponTrigger`-style component the author hasn't used elsewhere).

**Files:** Create `Class/Mesmerist/Tricks/CursedSanction.cs`; Modify `CommonTrickHelpers.cs`, `LocalizedStrings.json`

- [ ] **Step 1: Create the trick file**

```csharp
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils.Types;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Mesmerist.Utils;

namespace Mesmerist.Class.Mesmerist.Tricks
{
    // LAYER 3 — experimental (reactive trigger is a TODO)
    internal class CursedSanction
    {
        public static void Configure()
        {
            // The -4 debuff applied to an attacker (concrete).
            BuffConfigurator.New("CursedSanctionDebuff", Guids.CursedSanctionDebuffEffect)
                .SetDisplayName("CursedSanction.Name")
                .SetDescription("CursedSanction.Description")
                .SetIcon(AbilityRefs.Eyebite.Reference.Get().Icon)
                .AddStatBonus(ModifierDescriptor.UntypedStackable, false, StatType.AdditionalAttackBonus, -4)
                .AddStatBonus(ModifierDescriptor.UntypedStackable, false, StatType.SaveFortitude, -4)
                .AddStatBonus(ModifierDescriptor.UntypedStackable, false, StatType.SaveReflex, -4)
                .AddStatBonus(ModifierDescriptor.UntypedStackable, false, StatType.SaveWill, -4)
                .Configure();

            CommonTrickHelpers.CreateMasterfulTrick("CursedSanction",
                                                    "CursedSanction.Name",
                                                    "CursedSanction.Description",
                                                    AbilityRefs.Eyebite.Reference.Get().Icon,
                                                    Guids.CursedSanction,
                                                    Guids.CursedSanctionAbility,
                                                    Guids.CursedSanctionBuff);

            // TODO(verify-on-build): wire the implanted buff (Guids.CursedSanctionBuff) so that
            // when the subject is attacked, Guids.CursedSanctionDebuffEffect is applied to the
            // attacker for 1 min/level. This needs a "target attacked" trigger component, e.g.
            // a custom EntityFactComponentDelegate like the mod's AddCombatStare. Buff is
            // currently inert (debuff defined but not triggered).
            BuffConfigurator.For(Guids.CursedSanctionBuff)
                .Configure();
        }
    }
}
```

- [ ] **Step 2: Exclusion arrays** — append `Guids.CursedSanctionBuff` to BOTH arrays.
- [ ] **Step 3: Localization**

```json
    { "Key": "CursedSanction.Name", "ProcessTemplates": false, "enGB": "Cursed Sanction" },
    { "Key": "CursedSanction.Description", "enGB": "When the subject is attacked, the attacker is cursed, taking a -4 penalty on attack rolls and saving throws for 1 minute per mesmerist level. (Reactive trigger experimental.)" },
```

- [ ] **Step 4: Structural verify**

```bash
cd Mesmerist
grep -c "Guids.CursedSanctionBuff" Utils/CommonTrickHelpers.cs                # 2
grep -c "CursedSanction.Name\|CursedSanction.Description" LocalizedStrings.json # 2
```

- [ ] **Step 5: Commit**

```bash
git add Mesmerist/Class/Mesmerist/Tricks/CursedSanction.cs Mesmerist/Utils/CommonTrickHelpers.cs Mesmerist/LocalizedStrings.json
git commit -m "feat(trick): add Cursed Sanction (experimental, debuff defined)"
```

---

## Task 20: Vision of Blood (masterful trick, EXPERIMENTAL — reactive)

Tabletop: an enemy attacking the subject must save or be stunned 1 round. **Implementation:** define the stun debuff concretely; reactive trigger is a TODO (same pattern as Cursed Sanction).

**Files:** Create `Class/Mesmerist/Tricks/VisionOfBlood.cs`; Modify `CommonTrickHelpers.cs`, `LocalizedStrings.json`

- [ ] **Step 1: Create the trick file**

```csharp
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils.Types;
using Mesmerist.Utils;

namespace Mesmerist.Class.Mesmerist.Tricks
{
    // LAYER 3 — experimental (reactive trigger is a TODO)
    internal class VisionOfBlood
    {
        public static void Configure()
        {
            // CANDIDATE: reuse the game's Stunned buff via AddFacts; if a dedicated stun buff
            // ref isn't available, clone an existing stun effect. // TODO(verify-on-build)
            BuffConfigurator.New("VisionOfBloodStun", Guids.VisionOfBloodDebuff)
                .SetDisplayName("VisionOfBlood.Name")
                .SetDescription("VisionOfBlood.Description")
                .SetIcon(AbilityRefs.Eyebite.Reference.Get().Icon)
                .AddFacts(new() { BuffRefs.Stunned.Reference.Get() })
                .Configure();

            CommonTrickHelpers.CreateMasterfulTrick("VisionOfBlood",
                                                    "VisionOfBlood.Name",
                                                    "VisionOfBlood.Description",
                                                    AbilityRefs.Eyebite.Reference.Get().Icon,
                                                    Guids.VisionOfBlood,
                                                    Guids.VisionOfBloodAbility,
                                                    Guids.VisionOfBloodBuff);

            // TODO(verify-on-build): wire so that when the subject is attacked, the attacker
            // makes a Will save (DC = standard mesmerist DC) or gains Guids.VisionOfBloodDebuff
            // (stunned) for 1 round. Needs a "target attacked" trigger. Buff currently inert.
            BuffConfigurator.For(Guids.VisionOfBloodBuff)
                .Configure();
        }
    }
}
```

- [ ] **Step 2: Exclusion arrays** — append `Guids.VisionOfBloodBuff` to BOTH arrays.
- [ ] **Step 3: Localization**

```json
    { "Key": "VisionOfBlood.Name", "ProcessTemplates": false, "enGB": "Vision of Blood" },
    { "Key": "VisionOfBlood.Description", "enGB": "An enemy that attacks the subject is wracked by a horrific vision and must succeed at a Will save or be stunned for 1 round. (Reactive trigger experimental.)" },
```

- [ ] **Step 4: Structural verify**

```bash
cd Mesmerist
grep -c "Guids.VisionOfBloodBuff" Utils/CommonTrickHelpers.cs               # 2
grep -c "VisionOfBlood.Name\|VisionOfBlood.Description" LocalizedStrings.json # 2
```

- [ ] **Step 5: Commit**

```bash
git add Mesmerist/Class/Mesmerist/Tricks/VisionOfBlood.cs Mesmerist/Utils/CommonTrickHelpers.cs Mesmerist/LocalizedStrings.json
git commit -m "feat(trick): add Vision of Blood (experimental, stun defined)"
```

---

## Task 21: Final structural sweep + build handoff

**Files:** none (verification only)

- [ ] **Step 1: Confirm every new trick is registered and excluded**

```bash
cd Mesmerist
for t in FleetInShadows AstoundingAvoidance ReflectFear ShadowSplinter SpectralSmoke \
         CompelAlacrity LevitationBuffer Misdirection CursedSanction \
         GiftOfWill UmbralShield VisionOfBlood FakedDeath; do
  c=$(grep -c "$t.Configure();" Class/Mesmerist/Tricks/TrickSelection.cs)
  e=$(grep -c "Guids.${t}Buff" Utils/CommonTrickHelpers.cs)
  echo "$t  configure=$c  excluded=$e"
done
```
Expected: every line shows `configure=1 excluded=2`.

- [ ] **Step 2: Confirm every new stare is registered and dispatched**

```bash
cd Mesmerist
for s in Unaided Allure Sensed Oscillation Susceptibility; do
  c=$(grep -c "$s.Configure();" Class/Mesmerist/BoldStares/BoldStare.cs)
  d=$(grep -c "Guids.$s)" Class/Mesmerist/HypnoticStare.cs)
  echo "$s  configure=$c  dispatch=$d"
done
```
Expected: every line shows `configure=1 dispatch=1`.

- [ ] **Step 3: Confirm GUID uniqueness and localization completeness**

```bash
cd Mesmerist
echo "dup GUIDs (expect none):"; grep -oE '"[0-9a-f]{32}"' Utils/Guids.cs | sort | uniq -d
echo "localization keys added (expect 36 = 18 features x2):"; \
grep -cE '"(FleetInShadows|AstoundingAvoidance|ReflectFear|ShadowSplinter|SpectralSmoke|Unaided|Allure|Sensed|Oscillation|Susceptibility|GiftOfWill|UmbralShield|FakedDeath|CompelAlacrity|LevitationBuffer|Misdirection|CursedSanction|VisionOfBlood)\.(Name|Description)"' LocalizedStrings.json
```

- [ ] **Step 4: Validate `LocalizedStrings.json` still parses**

```bash
cd Mesmerist && python3 -c "import json;json.load(open('LocalizedStrings.json'));print('JSON OK')"
```
Expected: `JSON OK`.

- [ ] **Step 5: BUILD ON A MACHINE WITH WOTR (user action — cannot run here)**

1. Fix the hardcoded `TabletopTweaks-Core` `HintPath` in `Mesmerist/Mesmerist.csproj` to your local path.
2. `dotnet build Mesmerist.sln -c Release`.
3. Resolve compile errors flagged by `// TODO(verify-on-build)` and any CANDIDATE API mismatches (most likely: `FeatureRefs.ImprovedEvasion`, `AddBuffDescriptorImmunity`, `AddCannotBeFlanked`, `AddConditionImmunity`, `BuffRefs.InvisibilityBuff`, `BuffRefs.Stunned`, `AddDamageResistancePhysical`). For each, search BlueprintCore's configurator extensions for the correct method/ref name.
4. Launch, create a Mesmerist, and confirm new tricks appear in the trick selection and new stares in the bold-stare selection, then verify effects in combat.
5. If an experimental feature won't build, strip it via the EXPERIMENTAL regions (Task 10) and the `#region Layer3Experimental` GUIDs.

- [ ] **Step 6: Commit any final notes** (if Steps 1–4 surfaced fixes)

```bash
git add -A && git commit -m "chore: structural verification pass for new tricks and stares"
```

---

## Self-review notes

- **Spec coverage:** Every CORE feature (Fleet in Shadows, Astounding Avoidance, Reflect Fear, Shadow Splinter, Spectral Smoke, Unaided, Allure, Sensed) and every EXPERIMENTAL feature (Oscillation, Susceptibility, Gift of Will, Umbral Shield, Faked Death, Compel Alacrity, Levitation Buffer, Misdirection, Cursed Sanction, Vision of Blood) from spec §4 maps to a task. Skipped content (spec §5) is intentionally absent.
- **Isolation (spec §2.6):** Experimental features register only inside the marked regions (Task 10) and Layer-3 GUIDs sit in `#region Layer3Experimental` (Task 1) — strippable in one move.
- **Integration completeness:** every trick task touches the file + `TrickSelection.cs` + both `CommonTrickHelpers.cs` arrays + `LocalizedStrings.json`; every stare task touches the file + `BoldStare.cs` + `HypnoticStare.cs` dispatch + `LocalizedStrings.json`. Verified by Task 21 sweep.
- **No invented certainty:** every API not directly observed in the existing code is marked `CANDIDATE` / `TODO(verify-on-build)` with a fallback, never a silent guess.
