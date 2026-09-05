using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils.Types;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Mechanics;
using Mesmerist.Utils;

namespace Mesmerist.Class.Mesmerist.Tricks
{
    internal class Misdirection
    {
        // Tabletop: the mesmerist feints the creature attacking the subject, denying it its
        // Dexterity bonus to AC. WOTR has no feint mechanic at all - nothing in the game
        // assembly implements one - but it does model the outcome directly as the
        // LoseDexterityToAC unit condition, so the debuff is applied without a feint check.
        public static void Configure()
        {
            BuffConfigurator.New("MisdirectionFeintDebuff", Guids.MisdirectionBuffEffect)
                .SetDisplayName("MisdirectionFeint.Name")
                .SetDescription("MisdirectionFeint.Description")
                .SetIcon(IconLoader.GetOr("Misdirection", AbilityRefs.Displacement.Reference.Get().Icon))
                .AddBuffStatusCondition(condition: UnitCondition.LoseDexterityToAC)
                .Configure();

            CommonTrickHelpers.CreateTrick("Misdirection",
                                           "Misdirection.Name",
                                           "Misdirection.Description",
                                           AbilityRefs.Displacement.Reference.Get().Icon,
                                           Guids.Misdirection,
                                           Guids.MisdirectionAbility,
                                           Guids.MisdirectionBuff);

            // Fires the moment something attacks the subject: the attacker is caught out of
            // position for a round, and the implanted trick is spent.
            BuffConfigurator.For(Guids.MisdirectionBuff)
                .AddTargetAttackWithWeaponTrigger(
                    actionOnSelf: ActionsBuilder.New().RemoveSelf(),
                    actionsOnAttacker: ActionsBuilder.New()
                        .ApplyBuff(Guids.MisdirectionBuffEffect,
                                   ContextDuration.Fixed(1, DurationRate.Rounds)),
                    triggerBeforeAttack: true)
                .Configure();
        }
    }
}
