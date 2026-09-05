using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using Kingmaker.UnitLogic.FactLogic;
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
                .AddMechanicsFeature(AddMechanicsFeature.MechanicsFeatureType.CannotBeFlanked)
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
