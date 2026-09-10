using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.BasicEx;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Conditions.Builder;
using BlueprintCore.Conditions.Builder.BasicEx;
using BlueprintCore.Conditions.Builder.ContextEx;
using BlueprintCore.Utils;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Mesmerist.NewContexts;
using Mesmerist.Utils;
using TabletopTweaks.Core.NewActions;
using TabletopTweaks.Core.NewComponents;
using static Kingmaker.UnitLogic.Commands.Base.UnitCommand;

namespace Mesmerist.Class.Mesmerist.Mesmerist
{
    public class HypnoticStare
    {
        private static readonly string FeatName = "HypnoticStare";
        internal const string DisplayName = "HypnoticStare.Name";
        private static readonly string Description = "HypnoticStare.Description";

        public static void Configure()
        {

            BlueprintBuff hypnoticStareBuff = BuffConfigurator.New(FeatName + "Buff", Guids.HypnoticStareBuff)
                .SetDisplayName(DisplayName)
                .SetDescription(Description)
                .SetIcon(IconLoader.GetOr("HypnoticStare", AbilityRefs.Eyebite.Reference.Get().Icon))
                .AddSpellDescriptorComponent(SpellDescriptor.MindAffecting)
                .AddUniqueBuff()
                .SetFxOnStart("8de64fbe047abc243a9b4715f643739f")
                .AddContextCalculateAbilityParamsBasedOnClass(Guids.Mesmerist, statType: StatType.Charisma)
                .AddContextStatBonus(StatType.SaveWill, ContextValues.Rank(AbilityRankType.Default), ModifierDescriptor.UntypedStackable, 2, -1)
                .AddContextRankConfig(ContextRankConfigs.CharacterLevel(AbilityRankType.Default).WithCustomProgression((7, 2), (19, 3), (20, 5)))
                .Configure();

            // Shared effect logic between the manual swift-action ability and the
            // auto-cast trigger below (see HypnoticStare feature's AddFactContextActions).
            // Returns a fresh builder each call since a single ActionsBuilder instance
            // must not be reused across two separate .Build() call sites.
            //
            // Hypnotic Stare only ever has one active target at a time - there's no implemented
            // feat that lets it affect multiple targets simultaneously (Manifold Stare, despite
            // the name, grants extra Painful Stare ranks, not extra hypnotic stare targets - see
            // Manifold.cs). Nothing here has to enforce that: every buff applied below carries
            // AddUniqueBuff, and Kingmaker.UnitLogic.FactLogic.UniqueBuff registers the buff with
            // the *caster's* UnitPartUniqueBuffs, whose NewBuff() removes any earlier instance of
            // the same blueprint from that caster - wherever it currently sits. So re-staring
            // strips the whole package off the previous target automatically, and does it
            // per-caster, leaving a second mesmerist's stare alone.
            ActionsBuilder EffectActions() => ActionsBuilder.New()
                   .Conditional(
                               ConditionsBuilder.New().CasterHasFact(Guids.Disorientation),
                               ifTrue: ActionsBuilder.New().ApplyBuffPermanent(Guids.DisorientationBuff, true, false))
                   .Conditional(
                               ConditionsBuilder.New().CasterHasFact(Guids.Disquiet),
                               ifTrue: ActionsBuilder.New().ApplyBuffPermanent(Guids.DisquietBuff, true, false))
                   .Conditional(
                               ConditionsBuilder.New().CasterHasFact(Guids.Distracted),
                               ifTrue: ActionsBuilder.New().ApplyBuffPermanent(Guids.DistractedBuff, true, false))
                   .Conditional(
                               ConditionsBuilder.New().CasterHasFact(Guids.Infiltration),
                               ifTrue: ActionsBuilder.New().ApplyBuffPermanent(Guids.InfiltrationBuff, true, false))
                   .Conditional(
                               ConditionsBuilder.New().CasterHasFact(Guids.Lethality),
                               ifTrue: ActionsBuilder.New().ApplyBuffPermanent(Guids.LethalityBuff, true, false))
                   .Conditional(
                               ConditionsBuilder.New().CasterHasFact(Guids.Nightmare),
                               ifTrue: ActionsBuilder.New().ApplyBuffPermanent(Guids.NightmareBuff, true, false))
                   .Conditional(
                               ConditionsBuilder.New().CasterHasFact(Guids.PsychicInception),
                               ifTrue: ActionsBuilder.New().ApplyBuffPermanent(Guids.PsychicInceptionBuff, true, false))
                   .Conditional(
                               ConditionsBuilder.New().CasterHasFact(Guids.SappedMagic),
                               ifTrue: ActionsBuilder.New().ApplyBuffPermanent(Guids.SappedMagicBuff, true, false))
                   .Conditional(
                               ConditionsBuilder.New().CasterHasFact(Guids.Sluggishness),
                               ifTrue: ActionsBuilder.New().ApplyBuffPermanent(Guids.SluggishnessBuff, true, false))
                   .Conditional(
                               ConditionsBuilder.New().CasterHasFact(Guids.Timidity),
                               ifTrue: ActionsBuilder.New().ApplyBuffPermanent(Guids.TimidityBuff, true, false))
                   .Conditional(
                               ConditionsBuilder.New().CasterHasFact(Guids.Unaided),
                               ifTrue: ActionsBuilder.New().ApplyBuffPermanent(Guids.UnaidedBuff, true, false))
                   .Conditional(
                               ConditionsBuilder.New().CasterHasFact(Guids.Allure),
                               ifTrue: ActionsBuilder.New().ApplyBuffPermanent(Guids.AllureBuff, true, false))
                   .Conditional(
                               ConditionsBuilder.New().CasterHasFact(Guids.Sensed),
                               ifTrue: ActionsBuilder.New().ApplyBuffPermanent(Guids.SensedBuff, true, false))
                   .ApplyBuffPermanent(hypnoticStareBuff, true)
                   .Add<ContextActionApplyBuffRanks>(c =>
                   {
                       c.IsFromSpell = false;
                       c.m_Buff = BlueprintTool.GetRef<BlueprintBuffReference>(Guids.PainfulStareBuff);
                       c.Rank = new ContextValue()
                       {
                           ValueType = ContextValueType.Rank
                       };
                       c.Permanent = true;
                   });

            BlueprintAbility hypnoticStareAbility = AbilityConfigurator.New(FeatName + "Ability", Guids.HypnoticStareAbility)
                .SetDisplayName(DisplayName)
                .SetDescription(Description)
                .SetIcon(IconLoader.GetOr("HypnoticStare", AbilityRefs.EyebiteAbility.Reference.Get().Icon))
                .SetRange(AbilityRange.Close)
                .SetActionType(CommandType.Swift)
                .SetCanTargetSelf(false)
                .SetCanTargetFriends(false)
                .SetCanTargetEnemies(true)
                //.AddAbilityTargetIsAlly(true)
                .AddAbilityTargetHasFact(inverted: true, fromCaster: true, checkedFacts: [hypnoticStareBuff])
                //.SetSpellDescriptor(SpellDescriptor.MindAffecting)
                .SetNotOffensive(true)
                .AddAbilityEffectRunAction(actions: EffectActions())
                .AddContextRankConfig(ContextRankConfigs.FeatureRank(Guids.PainfulStare))
                .Configure();

            FeatureConfigurator.New(FeatName, Guids.HypnoticStare)
                .AddFacts(new() { hypnoticStareAbility })
                .SetDisplayName(DisplayName)
                .SetDescription(Description)
                .SetIcon(IconLoader.GetOr("HypnoticStare", AbilityRefs.EyebiteAbility.Reference.Get().Icon))
                .SetIsClassFeature()
                .SetReapplyOnLevelUp(false)
                // Auto-cast Hypnotic Stare on the nearest un-stared enemy each round while
                // playing real-time-with-pause. In turn-based mode the swift action stays
                // fully manual, matching tabletop rules and leaving the player's choice of
                // target in full control. A manually-targeted cast is unaffected either way -
                // this only fills in enemies nobody has stared yet.
                //
                // Must be AddFactContextActions, NOT AddNewRoundTrigger: the latter's
                // NewRoundTrigger listens for IUnitNewCombatRoundHandler, which is raised only
                // by TurnBased.Controllers.TurnController - and that whole controller is only
                // constructed while Turn-Based Mode is toggled on. AddFactContextActions runs
                // off ITickEachRound, which UnitTicksController drives on a 6-second timer in
                // real time (and TurnController also drives in turn-based), so it fires in
                // both modes.
                .AddContextRankConfig(ContextRankConfigs.FeatureRank(Guids.PainfulStare))
                .AddFactContextActions(newRound: ActionsBuilder.New().Conditional(
                    ConditionsBuilder.New().IsInTurnBasedCombat(negate: true),
                    ifTrue: ActionsBuilder.New().Add<ContextActionOnNearestEnemyWithoutFact>(c =>
                    {
                        c.FactToCheck = BlueprintTool.GetRef<BlueprintUnitFactReference>(Guids.HypnoticStareBuff);
                        c.Action = EffectActions().Build();
                    })))
                .Configure();

            FeatureConfigurator.New(FeatName + "Upgrade", Guids.HypnoticStareUpgrade)
                .SetDisplayName(DisplayName)
                .SetDescription(Description)
                .SetIcon(IconLoader.GetOr("HypnoticStare", AbilityRefs.EyebiteAbility.Reference.Get().Icon))
                .SetIsClassFeature()
                .Configure();

            FeatureConfigurator.New(FeatName + "PiercingGaze", Guids.HypnoticStarePiercingGaze)
                .SetDisplayName(DisplayName)
                .SetDescription(Description)
                .SetIcon(IconLoader.GetOr("HypnoticStare", AbilityRefs.EyebiteAbility.Reference.Get().Icon))
                .SetIsClassFeature()
                .Configure();
        }
    }
}
