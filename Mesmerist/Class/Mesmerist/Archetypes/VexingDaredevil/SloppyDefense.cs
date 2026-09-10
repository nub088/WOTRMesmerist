using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Conditions.Builder;
using BlueprintCore.Utils;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Components;
using TabletopTweaks.Core.NewActions;
using Mesmerist.NewConditions;
using Mesmerist.Utils;

namespace Mesmerist.Class.Mesmerist.Archetypes.VexingDaredevil
{
    /// <summary>
    /// Dazzling feint option (Vexing Daredevil, 3rd+). Tabletop: "+1 circumstance bonus on her
    /// next attack roll for every 5 mesmerist levels she possesses."
    ///
    /// Deviation: approximated as a flat +2 to-hit bonus rather than scaling per 5 levels, to
    /// avoid adding a second, differently-scoped rank config onto the shared HypnoticStareBuff.
    /// See BlindingStrike.cs for the shared trigger caveat.
    /// </summary>
    internal class SloppyDefense
    {
        private static readonly string FeatName = "SloppyDefense";
        internal const string DisplayName = "SloppyDefense.Name";
        private static readonly string Description = "SloppyDefense.Description";

        public static void Configure()
        {
            BuffConfigurator.New(FeatName + "BuffEffect", Guids.SloppyDefenseBuffEffect)
                .SetDisplayName(DisplayName)
                .SetDescription(Description)
                .SetIcon(IconLoader.GetOr(FeatName, AbilityRefs.TrueSeeing.Reference.Get().Icon))
                .AddStatBonus(ModifierDescriptor.Circumstance, false, StatType.AdditionalAttackBonus, 2)
                .Configure();

            FeatureConfigurator.New(FeatName, Guids.SloppyDefense)
                .SetDisplayName(DisplayName)
                .SetDescription(Description)
                .SetIcon(IconLoader.GetOr(FeatName, AbilityRefs.TrueSeeing.Reference.Get().Icon))
                .SetIsClassFeature()
                .Configure();

            BuffConfigurator.For(Guids.HypnoticStareBuff)
                .AddTargetAttackWithWeaponTrigger(
                    onlyHit: true,
                    actionsOnAttacker: ActionsBuilder.New().Conditional(
                        ConditionsBuilder.New()
                            .Add<ContextConditionInitiatorHasFact>(c =>
                            {
                                c.FactToCheck = BlueprintTool.GetRef<BlueprintUnitFactReference>(Guids.SloppyDefense);
                            })
                            .Add<ContextConditionOwnerHasBuffFromCaster>(c =>
                            {
                                c.Buff = BlueprintTool.GetRef<BlueprintBuffReference>(Guids.FeintedBuff);
                            }),
                        ifTrue: ActionsBuilder.New().ApplyBuff(
                            Guids.SloppyDefenseBuffEffect,
                            ContextDuration.Fixed(1, DurationRate.Rounds))))
                .Configure();
        }
    }
}
