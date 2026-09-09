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
    /// Dazzling feint option (Vexing Daredevil, 3rd+). Tabletop: a chosen combat maneuver
    /// against the feint target doesn't provoke an attack of opportunity this round.
    ///
    /// Deviation: the engine has no clean way to suppress a single upcoming maneuver's AoO from
    /// a reactive trigger, so this is approximated as a circumstance bonus to the daredevil's
    /// own combat maneuvers instead - she presses the advantage more effectively rather than
    /// literally avoiding the opportunity attack. See BlindingStrike.cs for the shared
    /// "fires on every hit against a hypnotically-stared target" caveat.
    /// </summary>
    internal class CombatManeuver
    {
        private static readonly string FeatName = "CombatManeuver";
        internal const string DisplayName = "CombatManeuver.Name";
        private static readonly string Description = "CombatManeuver.Description";

        public static void Configure()
        {
            BuffConfigurator.New(FeatName + "BuffEffect", Guids.CombatManeuverBuffEffect)
                .SetDisplayName(DisplayName)
                .SetDescription(Description)
                .SetIcon(IconLoader.GetOr(FeatName, AbilityRefs.TrueSeeing.Reference.Get().Icon))
                .AddStatBonus(ModifierDescriptor.Circumstance, false, StatType.AdditionalCMB, 4)
                .Configure();

            FeatureConfigurator.New(FeatName, Guids.CombatManeuver)
                .SetDisplayName(DisplayName)
                .SetDescription(Description)
                .SetIcon(IconLoader.GetOr(FeatName, AbilityRefs.TrueSeeing.Reference.Get().Icon))
                .SetIsClassFeature()
                .Configure();

            BuffConfigurator.For(Guids.HypnoticStareBuff)
                .AddTargetAttackWithWeaponTrigger(
                    onlyHit: true,
                    actionsOnAttacker: ActionsBuilder.New().Conditional(
                        ConditionsBuilder.New().Add<ContextConditionInitiatorHasFact>(c =>
                        {
                            c.FactToCheck = BlueprintTool.GetRef<BlueprintUnitFactReference>(Guids.CombatManeuver);
                        }),
                        ifTrue: ActionsBuilder.New().ApplyBuff(
                            Guids.CombatManeuverBuffEffect,
                            ContextDuration.Fixed(1, DurationRate.Rounds))))
                .Configure();
        }
    }
}
