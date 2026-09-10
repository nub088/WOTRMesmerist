using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Mechanics;
using Mesmerist.NewContexts;
using Mesmerist.Utils;

namespace Mesmerist.Class.Mesmerist.Archetypes.VexingDaredevil
{
    /// <summary>
    /// The feint action itself - the piece the archetype was missing.
    ///
    /// Wrath has no feint mechanic of its own (there is not one "Feint" string in
    /// Assembly-CSharp.dll), so before this the seven Dazzling Feint picks fired on *any* hit
    /// against a hypnotically-stared target, with no check and no cost. This adds the real
    /// thing: a Persuasion check against the target's feint DC, charged as a move action, whose
    /// success is what the picks now key off.
    ///
    /// It runs from a pre-attack trigger rather than as its own command, so it stays in sequence
    /// with the swing in real-time-with-pause without any scheduling - see
    /// <see cref="ContextActionFeintCheck"/> for why the command-queue route doesn't work.
    ///
    /// Improved Feint, Greater Feint and Greater Mesmerizing Feint all grant Persuasion bonuses,
    /// so they feed this check directly without needing to be wired in separately; Greater
    /// Mesmerizing Feint additionally waives the creature-type restriction inside the check.
    ///
    /// Configure after <see cref="ImprovedFeint.Configure"/> - the trigger is appended onto that
    /// feature, which every vexing daredevil gets at 3rd level alongside her first pick.
    /// </summary>
    internal class Feint
    {
        private static readonly string FeatName = "Feinted";
        internal const string DisplayName = "Feinted.Name";
        private static readonly string Description = "Feinted.Description";

        public static void Configure()
        {
            // The feinted target drops its Dexterity bonus to AC, which is what a successful
            // feint does in tabletop. One round rather than "until your next attack": in
            // real time there is no turn boundary to hang the shorter duration on, and it
            // matches what Greater Feint upgrades the duration to anyway.
            BuffConfigurator.New(FeatName + "Buff", Guids.FeintedBuff)
                .SetDisplayName(DisplayName)
                .SetDescription(Description)
                .SetIcon(IconLoader.GetOr(FeatName, AbilityRefs.Blindness.Reference.Get().Icon))
                .AddCondition(condition: UnitCondition.LoseDexterityToAC)
                .Configure();

            FeatureConfigurator.For(Guids.ImprovedFeint)
                .AddInitiatorAttackWithWeaponTrigger(
                    triggerBeforeAttack: true,
                    onlyOnFirstAttack: true,
                    action: ActionsBuilder.New().Add<ContextActionFeintCheck>(c =>
                    {
                        c.AppliedBuff = BlueprintTool.GetRef<BlueprintBuffReference>(Guids.FeintedBuff);
                        c.GreaterFeintFact = BlueprintTool.GetRef<BlueprintUnitFactReference>(Guids.GreaterMesmerizingFeint);
                        c.StareFact = BlueprintTool.GetRef<BlueprintUnitFactReference>(Guids.HypnoticStareBuff);
                        c.OnSuccess = ActionsBuilder.New()
                            .ApplyBuff(Guids.FeintedBuff, ContextDuration.Fixed(1, DurationRate.Rounds))
                            .Build();
                    }))
                .Configure();
        }
    }
}
