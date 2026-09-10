using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils.Types;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.Utility;
using Mesmerist.Utils;

namespace Mesmerist.Class.Mesmerist.Tricks
{
    internal class CompelAlacrity
    {
        // Tabletop: the subject may immediately move 10 ft (+5 ft per 5 mesmerist levels)
        // without provoking attacks of opportunity. Implemented as a short caster-only
        // dimension door granted by the implanted buff - a teleport provokes nothing, which
        // is the whole point of the trick.
        //
        // Deviation from tabletop, forced by the engine: distance is flat (the level 20
        // maximum) rather than scaling with mesmerist level. CustomRange is a static blueprint
        // field and cannot read caster level.
        private const float RangeFeet = 30f;

        public static void Configure()
        {
            CommonTrickHelpers.CreateTrick("CompelAlacrity",
                                           "CompelAlacrity.Name",
                                           "CompelAlacrity.Description",
                                           AbilityRefs.KiAbudantStep.Reference.Get().Icon,
                                           Guids.CompelAlacrity,
                                           Guids.CompelAlacrityAbility,
                                           Guids.CompelAlacrityBuff);

            // The blink the subject gains. AbilityCustomDimensionDoor carries a large set of
            // portal prefabs, FX and projectiles; copying the component from the monk's
            // Abundant Step gets a known-good visual instead of hand-wiring every asset link.
            AbilityConfigurator.New("CompelAlacrityDimensionDoor", Guids.CompelAlacrityDimensionDoorAbility)
                .CopyFrom(AbilityRefs.KiAbudantStep, typeof(AbilityCustomDimensionDoor))
                .SetDisplayName("CompelAlacrityDimensionDoor.Name")
                .SetDescription("CompelAlacrityDimensionDoor.Description")
                .SetIcon(IconLoader.GetOr("CompelAlacrity", AbilityRefs.KiAbudantStep.Reference.Get().Icon))
                .SetType(AbilityType.Supernatural)
                .SetRange(AbilityRange.Custom)
                .SetCustomRange(new Feet(RangeFeet))
                .SetActionType(UnitCommand.CommandType.Move)
                .SetCanTargetPoint(true)
                .SetCanTargetSelf(true)
                .SetCanTargetFriends(false)
                .SetCanTargetEnemies(false)
                .SetShouldTurnToTarget(true)
                .SetNotOffensive(true)
                // A mesmerist trick is discharged the moment it is triggered, so one blink
                // spends the implanted buff. AbilityCustomDimensionDoor is AbilityCustomLogic,
                // not an AbilityDeliverEffect, so AbilityExecutionProcess still applies the
                // ability's AbilityApplyEffect to the clicked target - which here is a point,
                // hence toCaster.
                .AddAbilityEffectRunAction(
                    ActionsBuilder.New().RemoveBuff(Guids.CompelAlacrityBuff, toCaster: true))
                .Configure();

            BuffConfigurator.For(Guids.CompelAlacrityBuff)
                .AddFacts(new() { Guids.CompelAlacrityDimensionDoorAbility })
                .Configure();
        }
    }
}
