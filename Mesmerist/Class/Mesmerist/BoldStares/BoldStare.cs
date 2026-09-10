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
        /// <remarks>
        /// The stares that impose a scaling numeric penalty all spell it as
        /// <c>AddContextStatBonus(stat, ContextValues.Rank(), UntypedStackable, 2, -1)</c>. Do not
        /// "tidy up" that trailing <c>2</c> (the <c>minimal</c> argument): BlueprintCore 2.8.6
        /// sets <c>HasMinimal = !minimal.HasValue</c>, i.e. inverted, so passing a value discards
        /// the clamp - which is what these need. Omitting it would set <c>HasMinimal = true,
        /// Minimal = 0</c>, and the engine's <c>Math.Max(value, Minimal)</c> would clamp every
        /// penalty away to zero.
        /// </remarks>
        internal static readonly Blueprint<BlueprintFeatureReference>[] All = [
            Guids.Disorientation, Guids.Disquiet, Guids.Distracted,
            Guids.Infiltration, Guids.Lethality, Guids.Nightmare,
            Guids.SappedMagic, Guids.Sluggishness, Guids.Timidity, Guids.PsychicInception,
            Guids.Unaided, Guids.Allure, Guids.Sensed];

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
