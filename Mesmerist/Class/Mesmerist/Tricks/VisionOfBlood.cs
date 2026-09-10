using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils.Types;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.UnitLogic.Mechanics;
using Mesmerist.Utils;

namespace Mesmerist.Class.Mesmerist.Tricks
{
    internal class VisionOfBlood
    {
        // Tabletop: when the subject is attacked, the attacker sees a horrifying vision and
        // must make a Will save or be stunned for 1 round. The trick is spent when it fires.
        public static void Configure()
        {
            CommonTrickHelpers.CreateMasterfulTrick("VisionOfBlood",
                                                    "VisionOfBlood.Name",
                                                    "VisionOfBlood.Description",
                                                    AbilityRefs.Eyebite.Reference.Get().Icon,
                                                    Guids.VisionOfBlood,
                                                    Guids.VisionOfBloodAbility,
                                                    Guids.VisionOfBloodBuff);

            // AddContextCalculateAbilityParamsBasedOnClass gives the save a real DC
            // (10 + 1/2 mesmerist level + Charisma) rather than defaulting to zero, since the
            // save is rolled from the buff's context rather than the implanting ability's.
            BuffConfigurator.For(Guids.VisionOfBloodBuff)
                .AddContextCalculateAbilityParamsBasedOnClass(
                    characterClass: Guids.Mesmerist, statType: StatType.Charisma)
                // onlyHit: false because the trigger is the attack, not a hit - the tabletop
                // trick fires (and is discharged) when the subject is attacked, whether or not
                // the blow lands.
                .AddTargetAttackWithWeaponTrigger(
                    onlyHit: false,
                    actionOnSelf: ActionsBuilder.New().RemoveSelf(),
                    actionsOnAttacker: ActionsBuilder.New()
                        .SavingThrow(
                            type: SavingThrowType.Will,
                            onResult: ActionsBuilder.New()
                                .ConditionalSaved(
                                    failed: ActionsBuilder.New()
                                        .ApplyBuff(BuffRefs.Stunned.Reference.Get(),
                                                   ContextDuration.Fixed(1, DurationRate.Rounds)),
                                    succeed: ActionsBuilder.New())))
                .Configure();
        }
    }
}
