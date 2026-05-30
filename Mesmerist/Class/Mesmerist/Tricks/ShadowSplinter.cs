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
