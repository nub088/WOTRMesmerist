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
    /// Dazzling feint option (Vexing Daredevil, 3rd+). Tabletop: on a critical threat against
    /// the feint target, gain a circumstance bonus to confirm the crit of +1 per 3 mesmerist
    /// levels.
    ///
    /// Deviation: the engine doesn't expose a per-fact bonus to critical confirmation rolls, so
    /// this is approximated as flat bonus direct damage on a hit, in the spirit of "a harder
    /// follow-up strike." See BlindingStrike.cs for the shared trigger caveat.
    /// </summary>
    internal class CriticalStrike
    {
        private static readonly string FeatName = "CriticalStrike";
        internal const string DisplayName = "CriticalStrike.Name";
        private static readonly string Description = "CriticalStrike.Description";

        public static void Configure()
        {
            FeatureConfigurator.New(FeatName, Guids.CriticalStrike)
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
                            c.FactToCheck = BlueprintTool.GetRef<BlueprintUnitFactReference>(Guids.CriticalStrike);
                        }),
                        ifTrue: ActionsBuilder.New().DealDamage(
                            new DamageTypeDescription() { Type = DamageType.Direct },
                            new ContextDiceValue()
                            {
                                DiceType = Kingmaker.RuleSystem.DiceType.D6,
                                DiceCountValue = ContextValues.Constant(2),
                            },
                            setFactAsReason: true,
                            ignoreCritical: true)))
                .Configure();
        }
    }
}
