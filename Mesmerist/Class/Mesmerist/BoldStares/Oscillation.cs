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
