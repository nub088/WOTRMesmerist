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
    public class Unaided
    {
        private static readonly string FeatName = "Unaided";
        internal const string DisplayName = "Unaided.Name";
        private static readonly string Description = "Unaided.Description";

        public static void Configure()
        {
            BuffConfigurator.New(FeatName + "Buff", Guids.UnaidedBuff)
                .SetDisplayName(DisplayName)
                .SetDescription(Description)
                .AddUniqueBuff()
                .SetIcon(IconLoader.GetOr("Unaided", BuffRefs.DebilitatingInjuryDisorientedEffectBuff.Reference.Get().Icon))
                // The tabletop effect is "the target can neither grant nor receive flanking
                // bonuses". WOTR has no hook for suppressing a unit's own flanking bonus, and
                // the one flanking mechanic that IS exposed - AddMechanicsFeature.CannotBeFlanked
                // - means the opposite: it makes the wearer immune to BEING flanked, which on a
                // debuff handed to an enemy is a straight buff for that enemy and a nerf to the
                // party's flanking builds. So the outcome is modelled instead: the party
                // effectively always has the flank, expressed as the stare penalty to the
                // target's AC.
                .AddContextStatBonus(StatType.AC, ContextValues.Rank(), ModifierDescriptor.UntypedStackable, 2, -1)
                .AddContextRankConfig(ContextRankConfigs.CharacterLevel(AbilityRankType.Default).WithCustomProgression((7, 2), (19, 3), (20, 5)))
                .Configure();

            FeatureConfigurator.New(FeatName, Guids.Unaided)
                .SetDisplayName(DisplayName)
                .SetDescription(Description)
                .SetIcon(IconLoader.GetOr("Unaided", AbilityRefs.TrueSeeing.Reference.Get().Icon))
                .SetIsClassFeature()
                .Configure();
        }
    }
}
