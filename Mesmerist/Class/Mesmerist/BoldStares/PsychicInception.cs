using System.Collections.Generic;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using Mesmerist.Utils;
using Kingmaker.Blueprints.Classes.Spells;
using TabletopTweaks.Core.NewComponents;
using Mesmerist.NewComponents.AbilitySpecific;
using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Components.TargetCheckers;

namespace Mesmerist.Class.Mesmerist.BoldStares
{
    public class PsychicInception
    {
        private static readonly string FeatName = "PsychicInception";
        internal const string DisplayName = "PsychicInception.Name";
        private static readonly string Description = "PsychicInception.Description";

        public static void Configure()
        {
            //TODO: Change CharacterLevel to ClassLevel(Mesmerist)
            FeatureConfigurator.New(FeatName, Guids.PsychicInception)
                .SetDisplayName(DisplayName)
                .SetDescription(Description)
                .SetIsClassFeature()
                .Configure();
            
            BuffConfigurator.New(FeatName + "Buff", Guids.PsychicInceptionBuff)
                .SetDisplayName(DisplayName)
                .SetDescription(Description)
                .AddUniqueBuff()
                .AddComponent<AddPsychicInception>()
                .SetIcon(IconLoader.GetOr("PsychicInception", BuffRefs.DebilitatingInjuryDisorientedEffectBuff.Reference.Get().Icon))
                .Configure();

            // --- FIX 2: gate the target-restriction bypass behind Psychic Inception (1e RAW) ---
            // 1e RAW only waives immunity to mind-affecting effects granted by creature
            // type - it does not let the mesmerist ignore a spell's own target-type
            // restriction (e.g. Hold Person still requires a humanoid target). Vanilla
            // enforces that restriction on these spells with AbilityTargetHasNoFactUnless
            // (blocking Animal/Vermin/Magical Beast targets). The RemoveComponents call
            // below used to strip that restriction unconditionally, which let every
            // caster in the game - not just mesmerists with this bold stare - target
            // these spells at animals/vermin/magical beasts. Swapping in our own copy of
            // the same restriction, gated on the PsychicInception fact via unlessFact,
            // keeps the restriction intact for everyone else while letting a mesmerist
            // with this bold stare bypass it - matching the commented-out approach this
            // replaces.
            //
            // To revert to the previous (unconditional) behavior, delete the
            // .AddAbilityTargetHasNoFactUnless(...) call in the loop below and leave only
            // the .RemoveComponents(...) call.
            List<Blueprint<BlueprintUnitFactReference>> lowIntTypes = new()
            {
                FeatureRefs.AnimalType.Cast<BlueprintUnitFactReference>(),
                FeatureRefs.VerminType.Cast<BlueprintUnitFactReference>(),
                FeatureRefs.MagicalBeastType.Cast<BlueprintUnitFactReference>(),
            };
            Blueprint<BlueprintUnitFactReference> psychicInceptionFact = BlueprintTool.GetRef<BlueprintUnitFactReference>(Guids.PsychicInception);

            foreach (var ability in new[]
            {
                AbilityRefs.Daze, AbilityRefs.CauseFear, AbilityRefs.Doom,
                AbilityRefs.CommandApproach, AbilityRefs.CommandFall, AbilityRefs.CommandFlee, AbilityRefs.CommandHalt,
                AbilityRefs.Castigate, AbilityRefs.HideousLaughter, AbilityRefs.HoldPerson, AbilityRefs.PhantasmalKiller,
                AbilityRefs.ConstrictingCoils, AbilityRefs.DominatePerson, AbilityRefs.HoldMonster,
                AbilityRefs.CommandGreaterApproach, AbilityRefs.CommandGreaterFall, AbilityRefs.CommandGreaterFlee, AbilityRefs.CommandGreaterHalt,
                AbilityRefs.HoldPersonMass, AbilityRefs.PowerWordKill, AbilityRefs.DominateMonster, AbilityRefs.Insanity,
            })
            {
                AbilityConfigurator.For(ability)
                    .RemoveComponents(c => c is AbilityTargetHasNoFactUnless)
                    .AddAbilityTargetHasNoFactUnless(checkedFacts: lowIntTypes, unlessFact: psychicInceptionFact)
                    .Configure();
            }
            // --- end FIX 2 -----------------------------------------------------------
        }
    }
}
