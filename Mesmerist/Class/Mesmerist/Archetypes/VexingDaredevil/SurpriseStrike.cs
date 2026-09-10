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
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Components;
using TabletopTweaks.Core.NewActions;
using Mesmerist.NewConditions;
using Mesmerist.Utils;

namespace Mesmerist.Class.Mesmerist.Archetypes.VexingDaredevil
{
    /// <summary>
    /// Dazzling feint option (Vexing Daredevil, 7th level+ only). Tabletop: "if her next attack
    /// hits, she can make another attack at her highest attack bonus - 5 as a free action."
    ///
    /// Deviation: injecting a genuine extra attack from a reactive on-hit trigger risks
    /// destabilizing the attack-resolution pipeline, so this is approximated as sizeable bonus
    /// direct damage on the hit instead - roughly what a follow-up strike would add. See
    /// BlindingStrike.cs for the shared trigger caveat.
    /// </summary>
    internal class SurpriseStrike
    {
        private static readonly string FeatName = "SurpriseStrike";
        internal const string DisplayName = "SurpriseStrike.Name";
        private static readonly string Description = "SurpriseStrike.Description";

        public static void Configure()
        {
            FeatureConfigurator.New(FeatName, Guids.SurpriseStrike)
                .SetDisplayName(DisplayName)
                .SetDescription(Description)
                .SetIcon(IconLoader.GetOr(FeatName, AbilityRefs.TrueSeeing.Reference.Get().Icon))
                .SetIsClassFeature()
                .AddPrerequisiteClassLevel(Guids.Mesmerist, 7)
                .Configure();

            BuffConfigurator.For(Guids.HypnoticStareBuff)
                .AddTargetAttackWithWeaponTrigger(
                    onlyHit: true,
                    actionOnSelf: ActionsBuilder.New().Conditional(
                        ConditionsBuilder.New()
                            .Add<ContextConditionInitiatorHasFact>(c =>
                            {
                                c.FactToCheck = BlueprintTool.GetRef<BlueprintUnitFactReference>(Guids.SurpriseStrike);
                            })
                            .Add<ContextConditionOwnerHasBuffFromCaster>(c =>
                            {
                                c.Buff = BlueprintTool.GetRef<BlueprintBuffReference>(Guids.FeintedBuff);
                            }),
                        ifTrue: ActionsBuilder.New().DealDamage(
                            new DamageTypeDescription() { Type = DamageType.Direct },
                            new ContextDiceValue()
                            {
                                DiceType = Kingmaker.RuleSystem.DiceType.D6,
                                DiceCountValue = ContextValues.Constant(2),
                                BonusValue = ContextValues.Constant(2),
                            },
                            setFactAsReason: true,
                            ignoreCritical: true)))
                .Configure();
        }
    }
}
