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
