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
