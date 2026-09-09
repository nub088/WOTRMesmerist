using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.Classes.Selection;
using BlueprintCore.Blueprints.References;
using Kingmaker.Blueprints.Classes;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.UnitLogic.Buffs;
using Mesmerist.Utils;

namespace Mesmerist.Class.Mesmerist.Features
{
    /// <summary>
    /// Force of Personality: a general feat that drives Will saves off Charisma rather than
    /// Wisdom. Not a mesmerist class feature - it is offered to any class - but it lives here
    /// because a Charisma-based save-or-suck class is what makes it worth having.
    /// </summary>
    class ForceOfPersonality
    {
        private static readonly string FeatName = "ForceOfPersonality";
        private static readonly string DisplayName = "ForceOfPersonality.Name";
        private static readonly string Description = "ForceOfPersonality.Description";

        public static void Configure()
        {
            FeatureConfigurator.New(FeatName, Guids.ForceOfPersonality, [FeatureGroup.Feat])
                .SetDisplayName(DisplayName)
                .SetDescription(Description)
                .SetIcon(IconLoader.GetOr("ForceOfPersonality", AbilityRefs.Guidance.Reference.Get().Icon))
                .SetRanks(1)
                // Will's base attribute is Wisdom; swapping it for Charisma is the whole feat.
                // ReplaceIfHigher keeps it from ever being a downgrade - the tabletop feat lets
                // you choose which modifier to add, and nobody would choose the smaller one.
                .AddReplaceStatBaseAttribute(
                    targetStat: StatType.SaveWill,
                    baseAttributeReplacement: StatType.Charisma,
                    replaceIfHigher: true,
                    replaceMod: BonusMod.AsIs)
                .AddPrerequisiteStatValue(StatType.Charisma, 13)
                .Configure();

            // No explicit BasicFeatSelection registration here on purpose. Creating the feature
            // with FeatureGroup.Feat already puts it in that selection; adding it again by hand
            // is what produced two Force of Personality entries at level up. The two Extra*
            // feats still register explicitly because they are built with
            // FeatureSelectionConfigurator, which does not do this.
        }
    }
}
