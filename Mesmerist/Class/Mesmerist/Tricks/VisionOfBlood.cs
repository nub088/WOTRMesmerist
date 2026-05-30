using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils.Types;
using Mesmerist.Utils;

namespace Mesmerist.Class.Mesmerist.Tricks
{
    // LAYER 3 — experimental (reactive trigger is a TODO)
    internal class VisionOfBlood
    {
        public static void Configure()
        {
            // CANDIDATE: reuse the game's Stunned buff via AddFacts; if a dedicated stun buff
            // ref isn't available, clone an existing stun effect. // TODO(verify-on-build)
            BuffConfigurator.New("VisionOfBloodStun", Guids.VisionOfBloodDebuff)
                .SetDisplayName("VisionOfBlood.Name")
                .SetDescription("VisionOfBlood.Description")
                .SetIcon(AbilityRefs.Eyebite.Reference.Get().Icon)
                .AddFacts(new() { BuffRefs.Stunned.Reference.Get() })
                .Configure();

            CommonTrickHelpers.CreateMasterfulTrick("VisionOfBlood",
                                                    "VisionOfBlood.Name",
                                                    "VisionOfBlood.Description",
                                                    AbilityRefs.Eyebite.Reference.Get().Icon,
                                                    Guids.VisionOfBlood,
                                                    Guids.VisionOfBloodAbility,
                                                    Guids.VisionOfBloodBuff);

            // TODO(verify-on-build): wire so that when the subject is attacked, the attacker
            // makes a Will save (DC = standard mesmerist DC) or gains Guids.VisionOfBloodDebuff
            // (stunned) for 1 round. Needs a "target attacked" trigger. Buff currently inert.
            BuffConfigurator.For(Guids.VisionOfBloodBuff)
                .Configure();
        }
    }
}
