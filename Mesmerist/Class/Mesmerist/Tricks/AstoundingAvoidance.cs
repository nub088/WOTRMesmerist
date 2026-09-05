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
