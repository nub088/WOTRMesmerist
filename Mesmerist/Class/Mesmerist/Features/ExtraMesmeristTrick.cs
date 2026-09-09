using BlueprintCore.Blueprints.CustomConfigurators.Classes.Selection;
using BlueprintCore.Blueprints.References;
using Kingmaker.Blueprints.Classes;
using Mesmerist.Class.Mesmerist.Tricks;
using Mesmerist.Utils;

namespace Mesmerist.Class.Mesmerist.Features
{
    /// <summary>
    /// Extra Mesmerist Trick: one more trick, as a feat. It is a selection rather than a plain
    /// feature so that taking it prompts the trick choice immediately, and it draws on
    /// TrickSelection.All so the feat can never offer a different list than the class does.
    /// </summary>
    class ExtraMesmeristTrick
    {
        private static readonly string FeatName = "ExtraMesmeristTrick";
        private static readonly string DisplayName = "ExtraMesmeristTrick.Name";
        private static readonly string Description = "ExtraMesmeristTrick.Description";

        public static void Configure()
        {
            FeatureSelectionConfigurator.New(FeatName, Guids.ExtraMesmeristTrick, [FeatureGroup.Feat])
                .SetDisplayName(DisplayName)
                .SetDescription(Description)
                .SetIcon(IconLoader.GetOr("ExtraMesmeristTrick", FeatureRefs.ArcanistExploits.Reference.Get().Icon))
                .AddToAllFeatures(TrickSelection.All)
                // The class grants its first trick at 1st level, so that is the real gate.
                .AddPrerequisiteClassLevel(Guids.Mesmerist, 1)
                .AddRecommendationHasClasses(recommendedClasses: [Guids.Mesmerist])
                // Repeatable: the tabletop feat may be taken more than once.
                .SetRanks(10)
                .Configure();

            // Remove-then-add keeps this idempotent. AddToAllFeatures appends without checking
            // for an existing entry, so if Configure runs a second time against the same
            // in-memory blueprint - a UMM reload, or the mod toggled off and on - the feat is
            // appended twice and shows up twice at level up until the game restarts.
            FeatureSelectionConfigurator.For(FeatureSelectionRefs.BasicFeatSelection)
                .RemoveFromAllFeatures(Guids.ExtraMesmeristTrick)
                .AddToAllFeatures(Guids.ExtraMesmeristTrick)
                .Configure();
        }
    }
}
