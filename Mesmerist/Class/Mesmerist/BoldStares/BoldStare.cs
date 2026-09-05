using BlueprintCore.Blueprints.References;
using Mesmerist.Utils;
using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using BlueprintCore.Blueprints.CustomConfigurators.Classes.Selection;

namespace Mesmerist.Class.Mesmerist.BoldStares
{
    public class BoldStare
    {
        private static readonly string FeatName = "BoldStare";
        internal const string DisplayName = "BoldStare.Name";
        private static readonly string Description = "BoldStare.Description";

        /// <summary>
        /// Every bold stare improvement. Shared with ExtraBoldStare so the feat and the class
        /// feature cannot offer different lists.
        /// </summary>
        internal static readonly Blueprint<BlueprintFeatureReference>[] All = [
            Guids.Disorientation, Guids.Disquiet, Guids.Distracted,
            Guids.Infiltration, Guids.Lethality, Guids.Nightmare,
            Guids.SappedMagic, Guids.Sluggishness, Guids.Timidity, Guids.PsychicInception,
            Guids.Unaided, Guids.Allure, Guids.Sensed,
            // EXPERIMENTAL (strip with the block above):
            Guids.Oscillation, Guids.Susceptibility];

        public static void Configure()
        {
            Disorientation.Configure();
            Disquiet.Configure();
            Distracted.Configure();
            Infiltration.Configure();
            Lethality.Configure();
            Nightmare.Configure();
            PsychicInception.Configure();
            SappedMagic.Configure();
            Sluggishness.Configure();
            Timidity.Configure();
            Manifold.Configure();
            Unaided.Configure();
            Allure.Configure();
            Sensed.Configure();

            // === EXPERIMENTAL (low-confidence — comment out this block to strip) ===
            Oscillation.Configure();
            Susceptibility.Configure();
            // === END EXPERIMENTAL ===

            FeatureSelectionConfigurator.New(FeatName, Guids.BoldStareSelection)
                .SetDisplayName(DisplayName)
                .SetDescription(Description)
                .SetIcon(IconLoader.GetOr("BoldStare", AbilityRefs.Blindness.Reference.Get().Icon))
                .SetIsClassFeature()
                .AddToAllFeatures(All)
                .Configure();
        }
    }
}
