using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils.Types;
using Kingmaker.Enums;
using Mesmerist.Utils;

namespace Mesmerist.Class.Mesmerist.Tricks
{
    // LAYER 3 — experimental (STUB)
    internal class CompelAlacrity
    {
        public static void Configure()
        {
            CommonTrickHelpers.CreateTrick("CompelAlacrity",
                                           "CompelAlacrity.Name",
                                           "CompelAlacrity.Description",
                                           AbilityRefs.Grace.Reference.Get().Icon,
                                           Guids.CompelAlacrity,
                                           Guids.CompelAlacrityAbility,
                                           Guids.CompelAlacrityBuff);

            // TODO(verify-on-build): real effect = burst of movement that provokes no AoO.
            // Stand-in: +10 ft speed. Replace with the scaffolded dimension-door approach
            // (Guids.CompelAlacrityDimensionDoorAbility/Feat) or drop this trick.
            BuffConfigurator.For(Guids.CompelAlacrityBuff)
                .AddBuffMovementSpeed(descriptor: ModifierDescriptor.UntypedStackable, value: 10, cappedMinimum: false, minimumCap: 0)
                .Configure();
        }
    }
}
