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
