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

            FeatureSelectionConfigurator.For(FeatureSelectionRefs.BasicFeatSelection)
                .AddToAllFeatures(Guids.ExtraBoldStare)
                .Configure();
        }
    }
}
