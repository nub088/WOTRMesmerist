using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils.Types;
using Kingmaker.UnitLogic;
using Mesmerist.Utils;

namespace Mesmerist.Class.Mesmerist.Tricks
{
    // LAYER 3 — experimental
    internal class UmbralShield
    {
        public static void Configure()
        {
            CommonTrickHelpers.CreateTrick("UmbralShield",
                                           "UmbralShield.Name",
                                           "UmbralShield.Description",
                                           AbilityRefs.Displacement.Reference.Get().Icon,
                                           Guids.UmbralShield,
                                           Guids.UmbralShieldAbility,
                                           Guids.UmbralShieldBuff);

            // CANDIDATE: AddConditionImmunity(UnitCondition.Dazzled). If the builder name
            // differs, fall back to AddBuffStatusCondition / a condition-immunity component.
            // TODO(verify-on-build)
            BuffConfigurator.For(Guids.UmbralShieldBuff)
                .AddConditionImmunity(UnitCondition.Dazzled)
                .Configure();
        }
    }
}
