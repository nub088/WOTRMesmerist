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
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Components;
using TabletopTweaks.Core.NewActions;
using Mesmerist.NewConditions;
using Mesmerist.Utils;

namespace Mesmerist.Class.Mesmerist.Archetypes.VexingDaredevil
{
    /// <summary>
    /// Dazzling feint option (Vexing Daredevil, 3rd+). Tabletop: "the vexing daredevil's weapon
    /// emits a bright flash of light into the opponent's eyes. If the vexing daredevil's next
    /// attack hits, the target must succeed at a Fortitude save or be blinded for 1 round."
    ///
    /// Hooked onto the shared HypnoticStareBuff (see HypnoticStare.cs), matching how Painful
    /// Stare's own on-hit trigger is wired, and gated on the target carrying this daredevil's
    /// FeintedBuff, so it fires only after a successful feint - see Feint.cs for the check.
    ///
    /// Remaining deviation: tabletop arms one specific "next attack," whereas the feint debuff
    /// lasts a round here, so every hit inside that round benefits.
    /// </summary>
    internal class BlindingStrike
    {
        private static readonly string FeatName = "BlindingStrike";
        internal const string DisplayName = "BlindingStrike.Name";
        private static readonly string Description = "BlindingStrike.Description";

        public static void Configure()
        {
            BuffConfigurator.New(FeatName + "Buff", Guids.BlindingStrikeBuff)
                .SetDisplayName(DisplayName)
                .SetDescription(Description)
                .SetIcon(IconLoader.GetOr(FeatName, AbilityRefs.TrueSeeing.Reference.Get().Icon))
                .AddFacts(new() { BuffRefs.BlindnessCombatBuff.Reference.Get() })
                .Configure();

            FeatureConfigurator.New(FeatName, Guids.BlindingStrike)
                .SetDisplayName(DisplayName)
                .SetDescription(Description)
                .SetIcon(IconLoader.GetOr(FeatName, AbilityRefs.TrueSeeing.Reference.Get().Icon))
                .SetIsClassFeature()
                .Configure();

            BuffConfigurator.For(Guids.HypnoticStareBuff)
                .AddTargetAttackWithWeaponTrigger(
                    onlyHit: true,
                    actionOnSelf: ActionsBuilder.New().Conditional(
                        ConditionsBuilder.New()
                            .Add<ContextConditionInitiatorHasFact>(c =>
                            {
                                c.FactToCheck = BlueprintTool.GetRef<BlueprintUnitFactReference>(Guids.BlindingStrike);
                            })
                            .Add<ContextConditionOwnerHasBuffFromCaster>(c =>
                            {
                                c.Buff = BlueprintTool.GetRef<BlueprintBuffReference>(Guids.FeintedBuff);
                            }),
                        ifTrue: ActionsBuilder.New().SavingThrow(
                            type: SavingThrowType.Fortitude,
                            onResult: ActionsBuilder.New().ConditionalSaved(
                                failed: ActionsBuilder.New().ApplyBuff(
                                    Guids.BlindingStrikeBuff,
                                    ContextDuration.Fixed(1, DurationRate.Rounds)),
                                succeed: ActionsBuilder.New()))))
                .Configure();
        }
    }
}
