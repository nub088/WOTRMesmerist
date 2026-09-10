using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Blueprints;
using Kingmaker.EntitySystem;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules.Abilities;
using Kingmaker.RuleSystem.Rules.Damage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.UnitLogic;
using Mesmerist.Utils;
using Kingmaker.Blueprints.Facts;
using BlueprintCore.Utils;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic.Parts;
using TabletopTweaks.Core.NewUnitParts;
using Kingmaker.Blueprints.Classes.Spells;

namespace Mesmerist.NewComponents.AbilitySpecific
{
    [AllowMultipleComponents]
    [TypeId("fe4eb0615e264792b1d3030a9c010277")]
    public class AddPsychicInception : UnitFactComponentDelegate
       
    {
        // --- FIX 1: descriptor scope (1e RAW) -------------------------------------
        // RAW: "whenever the mesmerist targets a creature under his hypnotic stare
        // with a spell, spell-like ability, or spell trick that could normally affect
        // that creature except that the creature is immune to mind-affecting effects,
        // that creature is not immune to the spell, spell-like ability, or spell trick
        // because of its immunity to mind-affecting effects. This ability doesn't allow
        // the mesmerist to affect creatures that are immune to the spell, spell-like
        // ability, or spell trick for a reason other than an immunity to mind-affecting
        // effects." Only the blanket MindAffecting immunity (Undead/Construct/Ooze type)
        // is bypassed here - narrower immunities to one specific descriptor (e.g. a
        // fear-only immunity that isn't a full mind-affecting immunity) are untouched.
        // Revert to the wider descriptor list below this comment block if you'd rather
        // keep the previous (broader) behavior.
        public override void OnTurnOn()
        {
            UnitPartSpellResistance unitpart = base.Owner.Ensure<UnitPartSpellResistance>();
            UnitPartIgnoreBuffDescriptorImmunity buffunitpart = base.Owner.Ensure<UnitPartIgnoreBuffDescriptorImmunity>();
            unitpart.IgnoreImmunity(SpellDescriptor.MindAffecting);
            buffunitpart.AddEntry(SpellDescriptor.MindAffecting, base.Fact);
        }

        public override void OnTurnOff()
        {
            UnitPartSpellResistance unitpart = base.Owner.Ensure<UnitPartSpellResistance>();
            UnitPartIgnoreBuffDescriptorImmunity buffunitpart = base.Owner.Ensure<UnitPartIgnoreBuffDescriptorImmunity>();
            unitpart.RestoreImmunity(SpellDescriptor.MindAffecting);
            buffunitpart.RemoveEntry(base.Fact);
        }
        // --- end FIX 1 -------------------------------------------------------------
    }
}
