using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Buffs;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.Utility;

namespace Mesmerist.NewContexts
{
    [TypeId("e9f2227fa8494042a3c5084415a26292")]
    public class ContextActionOnNearestEnemyWithoutFact : ContextAction
    {
        public override string GetCaption()
        {
            return "For the caster's current attack target, or the nearest enemy without a fact";
        }

        public override void RunAction()
        {
            UnitEntityData caster = base.Context.MaybeCaster;
            if (caster == null) return;

            // UnitTicksController drives ITickEachRound on every awake unit every 6 seconds of
            // game time whether or not anyone is fighting, so the combat gate has to be here.
            // Without it the mesmerist auto-stares anything that wanders into group memory
            // while the party is just walking around.
            if (!caster.IsInCombat || !caster.Descriptor.State.IsConscious) return;

            // Prefer whoever the caster is actually attacking (or last attacked) over pure
            // proximity - on-hit effects that key off this fact (Painful Stare, Vexing
            // Daredevil's Dazzling Feint) only trigger on hits against this target, so staring
            // the merely-nearest enemy is often wasted if that's not who's getting hit.
            UnitEntityData preferredTarget = caster.CombatState?.ManualTarget ?? caster.CombatState?.LastTarget;

            float rangeMeters = new Feet(MaxRangeFeet).Meters;
            UnitEntityData preferred = null;
            UnitEntityData nearest = null;
            float nearestDistance = float.MaxValue;

            // UnitGroupMemory.Enemies already filters out null and not-in-game units, but it
            // keeps corpses (and anything detected in the last 9 seconds) regardless of
            // distance or line of sight, so every other check below is ours to make.
            foreach (UnitGroupMemory.UnitInfo unitInfo in caster.Memory.Enemies)
            {
                UnitEntityData enemy = unitInfo.Unit;
                if (enemy.Descriptor.State.IsDead) continue;

                if (HasFactFromCaster(enemy, caster))
                {
                    // A living enemy already carries this caster's stare (a manual cast, or a
                    // previous auto-cast) - Hypnotic Stare only has one active target at a time,
                    // so leave that choice alone instead of hijacking it.
                    return;
                }

                // Only auto-cast on something the mesmerist could have legally clicked: seen
                // right now, still standing, and inside the ability's Close range.
                if (!unitInfo.Visible) continue;
                if (!enemy.Descriptor.State.IsConscious) continue;
                if (!IsWithinRange(caster, enemy, rangeMeters)) continue;

                if (enemy == preferredTarget) preferred = enemy;

                float distance = caster.DistanceTo(enemy);
                if (distance < nearestDistance)
                {
                    nearestDistance = distance;
                    nearest = enemy;
                }
            }

            UnitEntityData chosen = preferred ?? nearest;
            if (chosen == null) return;

            Main.log.Log($"[AutoHypnoticStare] {caster.CharacterName} -> {chosen.CharacterName}.");
            using (base.Context.GetDataScope(chosen))
            {
                this.Action.Run();
            }
        }

        // Whether this caster in particular has the stare on that enemy. A plain HasFact would
        // also see another mesmerist's stare, and the "already stared, leave it alone" rule would
        // then wedge every mesmerist in the party but the first. UniqueBuff is tracked per caster
        // (Kingmaker.UnitLogic.Parts.UnitPartUniqueBuffs), so two mesmerists genuinely can hold
        // separate stares, and this has to be caster-scoped to match.
        private bool HasFactFromCaster(UnitEntityData enemy, UnitEntityData caster)
        {
            BlueprintUnitFact fact = FactToCheck?.Get();
            if (fact == null) return false;

            foreach (Buff buff in enemy.Buffs.Enumerable)
            {
                if (ReferenceEquals(buff.Blueprint, fact) && buff.Context.MaybeCaster == caster)
                {
                    return true;
                }
            }
            return false;
        }

        // Mirrors how the engine measures ability range (see Kingmaker.AI.TargetInfo): the
        // blueprint range is centre-to-centre distance minus both units' physical radii.
        private static bool IsWithinRange(UnitEntityData caster, UnitEntityData enemy, float rangeMeters)
        {
            float corpulence = (caster.View != null ? caster.View.Corpulence : 0f)
                             + (enemy.View != null ? enemy.View.Corpulence : 0f);
            return caster.DistanceTo(enemy) <= rangeMeters + corpulence;
        }

        public ActionList Action;
        public BlueprintUnitFactReference FactToCheck;

        // AbilityRange.Close is a flat 30 ft in WOTR (BlueprintAbility.GetRange), which is
        // what HypnoticStareAbility is set to.
        public float MaxRangeFeet = 30f;
    }
}
