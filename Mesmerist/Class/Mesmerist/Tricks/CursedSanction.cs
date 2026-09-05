using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Mechanics;
using Mesmerist.Utils;

namespace Mesmerist.Class.Mesmerist.Tricks
{
    internal class CursedSanction
    {
        // Tabletop: when the subject is attacked, the attacker suffers -4 on attack rolls,
        // saves and checks for 1 minute per mesmerist level. The trick is spent when it fires.
        public static void Configure()
        {
            // The -4 penalty applied to whoever attacked the subject.
            BuffConfigurator.New("CursedSanctionDebuff", Guids.CursedSanctionDebuffEffect)
                .SetDisplayName("CursedSanction.Name")
                .SetDescription("CursedSanction.Description")
                .SetIcon(IconLoader.GetOr("CursedSanction", AbilityRefs.Eyebite.Reference.Get().Icon))
                .AddStatBonus(ModifierDescriptor.UntypedStackable, false, StatType.AdditionalAttackBonus, -4)
                .AddStatBonus(ModifierDescriptor.UntypedStackable, false, StatType.SaveFortitude, -4)
                .AddStatBonus(ModifierDescriptor.UntypedStackable, false, StatType.SaveReflex, -4)
                .AddStatBonus(ModifierDescriptor.UntypedStackable, false, StatType.SaveWill, -4)
                .Configure();

            CommonTrickHelpers.CreateMasterfulTrick("CursedSanction",
                                                    "CursedSanction.Name",
                                                    "CursedSanction.Description",
                                                    AbilityRefs.Eyebite.Reference.Get().Icon,
                                                    Guids.CursedSanction,
                                                    Guids.CursedSanctionAbility,
                                                    Guids.CursedSanctionBuff);

            // AddTargetAttackWithWeaponTrigger runs actionsOnAttacker against the attacker and
            // actionOnSelf against the buff's own holder, so the sanction lands on the attacker
            // and the trick removes itself - a mesmerist trick is spent once it triggers.
            // The rank config feeds ContextValues.Rank() for the 1 min/level duration.
            BuffConfigurator.For(Guids.CursedSanctionBuff)
                .AddContextRankConfig(ContextRankConfigs.ClassLevel([Guids.Mesmerist], type: AbilityRankType.Default))
                .AddTargetAttackWithWeaponTrigger(
                    actionOnSelf: ActionsBuilder.New().RemoveSelf(),
                    actionsOnAttacker: ActionsBuilder.New()
                        .ApplyBuff(Guids.CursedSanctionDebuffEffect,
                                   ContextDuration.Variable(ContextValues.Rank(), DurationRate.Minutes)))
                .Configure();
        }
    }
}
