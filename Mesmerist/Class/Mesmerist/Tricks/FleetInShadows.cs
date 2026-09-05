using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils.Types;
using Kingmaker.Enums;
using Mesmerist.Utils;

namespace Mesmerist.Class.Mesmerist.Tricks
{
    internal class FleetInShadows
    {
        public static void Configure()
        {
            CommonTrickHelpers.CreateTrick("FleetInShadows",
                                           "FleetInShadows.Name",
                                           "FleetInShadows.Description",
                                           AbilityRefs.Grace.Reference.Get().Icon,
                                           Guids.FleetInShadows,
                                           Guids.FleetInShadowsAbility,
                                           Guids.FleetInShadowsBuff);

            BuffConfigurator.For(Guids.FleetInShadowsBuff)
                .AddBuffMovementSpeed(descriptor: ModifierDescriptor.UntypedStackable, value: 30, cappedMinimum: false, minimumCap: 0)
                .Configure();
        }
    }
}
