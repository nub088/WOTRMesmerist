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
    internal class LevitationBuffer
    {
        // Tabletop: the subject briefly lifts adjacent enemies off the ground, shoving them
        // back. WOTR has no vertical position for units, so there is nothing to lift them
        // onto - but ContextActionPush is the engine's real forced-movement action (the same
        // one bull rush uses), so the shove itself is expressible. Implemented as an ability
        // the implanted trick grants to its subject: a burst around the subject that pushes
        // every enemy caught in it away, without provoking on the way out.
        private const float BurstRadiusFeet = 10f;
        private const int PushDistanceFeet = 5;

        public static void Configure()
        {
            CommonTrickHelpers.CreateTrick("LevitationBuffer",
                                           "LevitationBuffer.Name",
                                           "LevitationBuffer.Description",
                                           AbilityRefs.Grace.Reference.Get().Icon,
                                           Guids.LevitationBuffer,
                                           Guids.LevitationBufferAbility,
                                           Guids.LevitationBufferBuff);

            AbilityConfigurator.New("LevitationBufferPush", Guids.LevitationBufferPushAbility)
                .SetDisplayName("LevitationBufferPush.Name")
                .SetDescription("LevitationBufferPush.Description")
                .SetIcon(IconLoader.GetOr("LevitationBuffer", AbilityRefs.Grace.Reference.Get().Icon))
                .SetType(AbilityType.Supernatural)
                .SetRange(AbilityRange.Personal)
                .SetActionType(UnitCommand.CommandType.Standard)
                .SetCanTargetSelf(true)
                .SetCanTargetPoint(false)
                .SetCanTargetFriends(false)
                .SetCanTargetEnemies(false)
                .AddAbilityTargetsAround(radius: new Feet(BurstRadiusFeet), targetType: TargetType.Enemy)
                .AddAbilityEffectRunAction(
                    ActionsBuilder.New()
                        .Push(distance: ContextValues.Constant(PushDistanceFeet),
                              provokeAttackOfOpportunity: false))
                .Configure();

            BuffConfigurator.For(Guids.LevitationBufferBuff)
                .AddFacts(new() { Guids.LevitationBufferPushAbility })
                .Configure();
        }
    }
}
