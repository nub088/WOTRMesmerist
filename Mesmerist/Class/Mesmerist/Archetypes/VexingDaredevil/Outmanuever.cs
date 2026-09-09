using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Conditions.Builder;
using BlueprintCore.Utils;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Components;
using TabletopTweaks.Core.NewActions;
using Mesmerist.Utils;

namespace Mesmerist.Class.Mesmerist.Archetypes.VexingDaredevil
{
    /// <summary>
    /// Dazzling feint option (Vexing Daredevil, 3rd+). Tabletop (author's spelling of the
    /// blueprint name kept as "Outmanuever" to match the pre-existing GUIDs): "gains a +4
    /// circumstance bonus on Acrobatics checks to move through the target's space or threatened
    /// area for 1 round" after a hit. Acrobatics maps to the Mobility skill in this engine.
    /// See BlindingStrike.cs for the shared trigger caveat.
    /// </summary>
    internal class Outmanuever
    {
        private static readonly string FeatName = "Outmanuever";
        internal const string DisplayName = "Outmanuever.Name";
        private static readonly string Description = "Outmanuever.Description";

        public static void Configure()
        {
            BuffConfigurator.New(FeatName + "BuffEffect", Guids.OutmanueverBuffEffect)
                .SetDisplayName(DisplayName)
                .SetDescription(Description)
                .SetIcon(IconLoader.GetOr(FeatName, AbilityRefs.TrueSeeing.Reference.Get().Icon))
                .AddStatBonus(ModifierDescriptor.Circumstance, false, StatType.SkillMobility, 4)
                .Configure();

            FeatureConfigurator.New(FeatName, Guids.Outmanuever)
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
                            c.FactToCheck = BlueprintTool.GetRef<BlueprintUnitFactReference>(Guids.Outmanuever);
                        }),
                        ifTrue: ActionsBuilder.New().ApplyBuff(
                            Guids.OutmanueverBuffEffect,
                            ContextDuration.Fixed(1, DurationRate.Rounds))))
                .Configure();
        }
    }
}
