using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils.Types;
using Mesmerist.Utils;

namespace Mesmerist.Class.Mesmerist.Tricks
{
    // LAYER 3 — experimental (STUB: no functional effect yet)
    internal class Misdirection
    {
        public static void Configure()
        {
            CommonTrickHelpers.CreateTrick("Misdirection",
                                           "Misdirection.Name",
                                           "Misdirection.Description",
                                           AbilityRefs.Displacement.Reference.Get().Icon,
                                           Guids.Misdirection,
                                           Guids.MisdirectionAbility,
                                           Guids.MisdirectionBuff);

            // TODO(verify-on-build): real effect feints the attacker (denies Dex to AC).
            // Needs a trigger + feint action (scaffolded Guids.MisdirectionFeintFeat).
            // Buff is currently inert. Implement or drop before release.
            BuffConfigurator.For(Guids.MisdirectionBuff)
                .Configure();
        }
    }
}
