using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils.Types;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Mesmerist.Utils;

namespace Mesmerist.Class.Mesmerist.Tricks
{
    // LAYER 3 — experimental (reactive trigger is a TODO)
    internal class CursedSanction
    {
        public static void Configure()
        {
            // The -4 debuff applied to an attacker (concrete).
            BuffConfigurator.New("CursedSanctionDebuff", Guids.CursedSanctionDebuffEffect)
                .SetDisplayName("CursedSanction.Name")
                .SetDescription("CursedSanction.Description")
                .SetIcon(AbilityRefs.Eyebite.Reference.Get().Icon)
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

            // TODO(verify-on-build): wire the implanted buff (Guids.CursedSanctionBuff) so that
            // when the subject is attacked, Guids.CursedSanctionDebuffEffect is applied to the
            // attacker for 1 min/level. This needs a "target attacked" trigger component, e.g.
            // a custom EntityFactComponentDelegate like the mod's AddCombatStare. Buff is
            // currently inert (debuff defined but not triggered).
            BuffConfigurator.For(Guids.CursedSanctionBuff)
                .Configure();
        }
    }
}
