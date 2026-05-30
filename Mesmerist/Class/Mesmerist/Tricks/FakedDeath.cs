using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils.Types;
using Mesmerist.Utils;

namespace Mesmerist.Class.Mesmerist.Tricks
{
    // LAYER 3 — experimental
    internal class FakedDeath
    {
        public static void Configure()
        {
            CommonTrickHelpers.CreateMasterfulTrick("FakedDeath",
                                                    "FakedDeath.Name",
                                                    "FakedDeath.Description",
                                                    AbilityRefs.Displacement.Reference.Get().Icon,
                                                    Guids.FakedDeath,
                                                    Guids.FakedDeathAbility,
                                                    Guids.FakedDeathBuff);

            // CANDIDATE: BuffRefs.InvisibilityBuff. If the ref name differs, use the
            // game's invisibility buff blueprint. // TODO(verify-on-build)
            BuffConfigurator.For(Guids.FakedDeathBuff)
                .AddFacts(new() { BuffRefs.InvisibilityBuff.Reference.Get() })
                .Configure();
        }
    }
}
