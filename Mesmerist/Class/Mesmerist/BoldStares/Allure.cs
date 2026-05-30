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
