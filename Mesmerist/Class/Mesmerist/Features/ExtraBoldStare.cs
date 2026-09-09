using BlueprintCore.Blueprints.CustomConfigurators.Classes.Selection;
using BlueprintCore.Blueprints.References;
using Kingmaker.Blueprints.Classes;
using Mesmerist.Class.Mesmerist.BoldStares;
using Mesmerist.Utils;

namespace Mesmerist.Class.Mesmerist.Features
{
    /// <summary>
    /// Extra Bold Stare: one more bold stare improvement, as a feat. Shares BoldStare.All with
    /// the class feature for the same reason ExtraMesmeristTrick shares TrickSelection.All.
    /// </summary>
    class ExtraBoldStare
    {
        private static readonly string FeatName = "ExtraBoldStare";
        private static readonly string DisplayName = "ExtraBoldStare.Name";
        private static readonly string Description = "ExtraBoldStare.Description";

        public static void Configure()
        {
            FeatureSelectionConfigurator.New(FeatName, Guids.ExtraBoldStare, [FeatureGroup.Feat])
                .SetDisplayName(DisplayName)
                .SetDescription(Description)
                .SetIcon(IconLoader.GetOr("ExtraBoldStare", AbilityRefs.Blindness.Reference.Get().Icon))
                .AddToAllFeatures(BoldStare.All)
                // Bold stare is a 3rd-level class feature; there is nothing to add before then.
                .AddPrerequisiteClassLevel(Guids.Mesmerist, 3)
                .AddRecommendationHasClasses(recommendedClasses: [Guids.Mesmerist])
                .SetRanks(5)
                .Configure();

            // Remove-then-add keeps this idempotent. AddToAllFeatures appends without checking
            // for an existing entry, so if Configure runs a second time against the same
            // in-memory blueprint - a UMM reload, or the mod toggled off and on - the feat is
            // appended twice and shows up twice at level up until the game restarts.
            FeatureSelectionConfigurator.For(FeatureSelectionRefs.BasicFeatSelection)
                .RemoveFromAllFeatures(Guids.ExtraBoldStare)
                .AddToAllFeatures(Guids.ExtraBoldStare)
                .Configure();
        }
    }
}
