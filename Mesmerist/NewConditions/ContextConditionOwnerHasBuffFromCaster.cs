using Kingmaker.Blueprints;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.UnitLogic.Buffs;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Mechanics.Conditions;

namespace Mesmerist.NewConditions
{
    /// <summary>
    /// True when the fact's *owner* carries a given buff applied by the fact's own caster.
    ///
    /// BlueprintCore's <c>HasBuffFromCaster</c> checks the current action target, which is no
    /// good for the Dazzling Feint picks: they hang off HypnoticStareBuff, and half of them run
    /// their actions on the attacker (CombatManeuver, Outmanuever, SloppyDefense) while the
    /// other half run on the stared enemy. This condition ignores the action target entirely and
    /// asks about the buff owner, so one form works in both lists.
    ///
    /// Caster-scoped for the same reason <see cref="Mesmerist.NewContexts.ContextActionOnNearestEnemyWithoutFact"/>
    /// is: two mesmerists can hold separate stares on the same enemy, and a plain HasFact would
    /// let one daredevil's dazzling feints fire off the other one's feint.
    /// </summary>
    [TypeId("0dc9a3b1f4d24c1ea1d90a4e0f4f9b17")]
    public class ContextConditionOwnerHasBuffFromCaster : ContextCondition
    {
        public override string GetConditionCaption()
        {
            return "Fact owner has buff from this fact's caster";
        }

        public override bool CheckCondition()
        {
            UnitEntityData owner = base.Context.MaybeOwner;
            UnitEntityData caster = base.Context.MaybeCaster;
            if (owner == null || caster == null) return false;

            BlueprintBuff buff = Buff?.Get();
            if (buff == null) return false;

            foreach (Buff active in owner.Buffs.Enumerable)
            {
                if (ReferenceEquals(active.Blueprint, buff) && active.Context.MaybeCaster == caster)
                {
                    return true;
                }
            }
            return false;
        }

        public BlueprintBuffReference Buff;
    }
}
