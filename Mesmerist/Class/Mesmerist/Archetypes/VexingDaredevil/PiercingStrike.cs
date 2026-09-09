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
    /// Dazzling feint option (Vexing Daredevil, 3rd+). Tabletop: if the next attack hits and
    /// Painful Stare applies, its damage dice step up from d6s to d8s.
    ///
    /// Deviation: rewiring Painful Stare's own damage-dice rank config from a reactive trigger
    /// on a different buff is high risk, so this is approximated as flat bonus direct damage on
    /// a hit instead - roughly the average gain from a d6-to-d8 step. See BlindingStrike.cs for
    /// the shared trigger caveat.
    /// </summary>
    internal class PiercingStrike
    {
        private static readonly string FeatName = "PiercingStrike";
        internal const string DisplayName = "PiercingStrike.Name";
        private static readonly string Description = "PiercingStrike.Description";

        public static void Configure()
        {
            FeatureConfigurator.New(FeatName, Guids.PiercingStrike)
                .SetDisplayName(DisplayName)
                .SetDescription(Description)
                .SetIcon(IconLoader.GetOr(FeatName, AbilityRefs.TrueSeeing.Reference.Get().Icon))
                .SetIsClassFeature()
                .Configure();

            BuffConfigurator.For(Guids.HypnoticStareBuff)
                .AddTargetAttackWithWeaponTrigger(
                    onlyHit: true,
                    actionOnSelf: ActionsBuilder.New().Conditional(
                        ConditionsBuilder.New().Add<ContextConditionInitiatorHasFact>(c =>
                        {
                            c.FactToCheck = BlueprintTool.GetRef<BlueprintUnitFactReference>(Guids.PiercingStrike);
                        }),
                        ifTrue: ActionsBuilder.New().DealDamage(
                            new DamageTypeDescription() { Type = DamageType.Direct },
                            new ContextDiceValue()
                            {
                                DiceType = Kingmaker.RuleSystem.DiceType.One,
                                DiceCountValue = ContextValues.Constant(0),
                                BonusValue = ContextValues.Constant(1),
                            },
                            setFactAsReason: true,
                            ignoreCritical: true)))
                .Configure();
        }
    }
}
