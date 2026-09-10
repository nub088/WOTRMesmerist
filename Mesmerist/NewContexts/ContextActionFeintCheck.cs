using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Designers;
using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic.Buffs;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Mechanics.Actions;
using TurnBased.Controllers;
using UnitCommand = Kingmaker.UnitLogic.Commands.Base.UnitCommand;

namespace Mesmerist.NewContexts
{
    /// <summary>
    /// Resolves a feint attempt against the current target: a Persuasion check against the
    /// target's feint DC, charged as a move action, running <see cref="OnSuccess"/> when it
    /// lands.
    ///
    /// Meant to be driven from an AddInitiatorAttackWithWeaponTrigger with
    /// <c>triggerBeforeAttack: true, onlyOnFirstAttack: true</c>, which fires from
    /// AbstractWeaponTrigger.AboutToTrigger - i.e. from
    /// IInitiatorRulebookHandler&lt;RuleAttackWithWeapon&gt;.OnEventAboutToTrigger, before the
    /// attack rolls resolve. That makes the feint part of the swing rather than a separate
    /// command, which is the only way to get "feint, then attack" to stay in sequence with
    /// real-time-with-pause combat: the engine has no rotation scripting for player units, and
    /// issuing the two as commands runs into UnitCommands.Run clearing the queue and a
    /// move-slot command cancelling the attack's approach.
    ///
    /// Whether the debuff applied by <see cref="OnSuccess"/> lands in time to affect the very
    /// swing that triggered it is NOT verified in-game - see the ordering log below.
    /// </summary>
    [TypeId("6b1f0e2c8ad5417f9c5e3a7d24b8e6f0")]
    public class ContextActionFeintCheck : ContextAction
    {
        public override string GetCaption()
        {
            return "Attempt a feint (Persuasion) against the target";
        }

        public override void RunAction()
        {
            UnitEntityData caster = base.Context.MaybeCaster;
            UnitEntityData target = base.Target?.Unit;
            if (caster == null || target == null) return;

            if (!caster.IsInCombat || !caster.Descriptor.State.IsConscious) return;
            if (target.Descriptor.State.IsDead) return;

            // Already feinted this target and the debuff is still up - don't re-roll and don't
            // charge the move action a second time.
            if (HasBuffFromCaster(target, caster)) return;

            // ---- ordering diagnostic -------------------------------------------------------
            // The open question this feature can't answer from the decompile: by the time a
            // pre-attack trigger runs, has the attack command already called
            // UnitEntityData.SpendAction? In real time the Standard branch sets
            // `cooldown.MoveAction = 3f` alongside the standard cooldown, so if the attack
            // spends first, UsedOneMoveAction() is already true here and RequireMoveAction
            // would gate the feint out permanently. Read this line in the UMM log after one
            // fight and set RequireMoveAction accordingly.
            var cooldown = caster.CombatState.Cooldown;
            Main.log.Log(
                $"[VexingFeint] {caster.CharacterName} -> {target.CharacterName}: " +
                $"moveCd={cooldown.MoveAction:F2} standardCd={cooldown.StandardAction:F2} " +
                $"usedOneMove={caster.UsedOneMoveAction()} usedStandard={caster.UsedStandardAction()} " +
                $"tbm={CombatController.IsInTurnBasedCombat()}");
            // --------------------------------------------------------------------------------

            if (RequireMoveAction && caster.UsedOneMoveAction())
            {
                Main.log.Log("[VexingFeint] skipped: no move action available.");
                return;
            }

            int dc = GetFeintDC(caster, target);
            if (dc < 0)
            {
                Main.log.Log($"[VexingFeint] skipped: {target.CharacterName} cannot be feinted (mindless).");
                return;
            }

            // The feint costs the move action whether or not it lands - that is the whole point
            // of Improved Feint being a move action rather than free. Charged before the roll so
            // a failed feint still eats the reposition.
            caster.SpendAction(UnitCommand.CommandType.Move, isFullRound: false, timeSinceCommandStart: 0f);

            RuleStatCheck check = RuleStatCheck.Create(caster, StatType.SkillPersuasion, dc);
            check.ShowAnyway = true;
            check = GameHelper.TriggerStatCheck(check, base.Context);

            Main.log.Log($"[VexingFeint] DC {dc}, rolled {check.RollResult}: {(check.Success ? "success" : "failure")}.");

            if (check.Success)
            {
                OnSuccess.Run();
            }
        }

        /// <summary>
        /// Feint DC: 10 + the better of the target's Sense Motive and its BAB + Wis modifier.
        /// Sense Motive is folded into Perception in this engine, so SkillPerception stands in.
        /// Returns -1 when the target cannot be feinted at all.
        ///
        /// Deviation: tabletop also adds +4 for a non-humanoid target. Creature type isn't
        /// plumbed here, so only the Intelligence-based tiers below are applied - the animal
        /// and mindless cases, which are the ones Greater Mesmerizing Feint exists to waive.
        /// </summary>
        private int GetFeintDC(UnitEntityData caster, UnitEntityData target)
        {
            var stats = target.Descriptor.Stats;
            int senseMotive = stats.SkillPerception.ModifiedValue;
            int martial = stats.BaseAttackBonus.ModifiedValue + stats.Wisdom.Bonus;
            int dc = 10 + (senseMotive > martial ? senseMotive : martial);

            // Greater Mesmerizing Feint: "you can feint against non-humanoid, animal-minded, and
            // even mindless creatures as long as they are the subject of your hypnotic stare."
            bool waived = GreaterFeintFact != null
                && caster.Descriptor.Facts.Contains(f => ReferenceEquals(f.Blueprint, GreaterFeintFact.Get()))
                && StareFact != null
                && HasFactFromCaster(target, StareFact.Get(), caster);
            if (waived) return dc;

            if (!stats.Intelligence.Enabled) return -1;   // mindless
            if (stats.Intelligence.ModifiedValue <= 2) dc += 8;   // animal-minded

            return dc;
        }

        private bool HasBuffFromCaster(UnitEntityData unit, UnitEntityData caster)
        {
            BlueprintBuff buff = AppliedBuff?.Get();
            if (buff == null) return false;

            foreach (Buff active in unit.Buffs.Enumerable)
            {
                if (ReferenceEquals(active.Blueprint, buff) && active.Context.MaybeCaster == caster)
                {
                    return true;
                }
            }
            return false;
        }

        private static bool HasFactFromCaster(UnitEntityData unit, BlueprintUnitFact fact, UnitEntityData caster)
        {
            if (fact == null) return false;

            foreach (Buff active in unit.Buffs.Enumerable)
            {
                if (ReferenceEquals(active.Blueprint, fact) && active.Context.MaybeCaster == caster)
                {
                    return true;
                }
            }
            return false;
        }

        public ActionList OnSuccess;

        /// <summary>The debuff <see cref="OnSuccess"/> applies; used to avoid re-feinting.</summary>
        public BlueprintBuffReference AppliedBuff;

        /// <summary>Greater Mesmerizing Feint, which waives the creature-type restrictions.</summary>
        public BlueprintUnitFactReference GreaterFeintFact;

        /// <summary>HypnoticStareBuff - Greater Mesmerizing Feint only waives them while stared.</summary>
        public BlueprintUnitFactReference StareFact;

        /// <summary>
        /// Gate the feint on actually having a move action left, rather than only charging one.
        /// Defaults off until the ordering log above confirms the gate can ever pass - see the
        /// comment there. Charging without gating still costs the daredevil her reposition.
        /// </summary>
        public bool RequireMoveAction = false;
    }
}
