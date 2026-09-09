using BlueprintCore.Blueprints.CustomConfigurators.Classes.Selection;
using BlueprintCore.Blueprints.References;
using Mesmerist.Utils;

namespace Mesmerist.Class.Mesmerist.Archetypes.VexingDaredevil
{
    /// <summary>
    /// Vexing Daredevil's Dazzling Feint: picked at 3rd level and again every 4 levels
    /// thereafter (mirrors Bold Stare's own cadence), replacing bold stare entirely. Each pick
    /// hooks an on-hit effect onto the shared HypnoticStareBuff - see BlindingStrike.cs for the
    /// approximation this whole set shares (fires on every hit against a stared target, rather
    /// than only after a successful feint).
    /// </summary>
    public class DazzlingFeint
    {
        private static readonly string FeatName = "DazzlingFeint";
        internal const string DisplayName = "DazzlingFeint.Name";
        private static readonly string Description = "DazzlingFeint.Description";

        public static void Configure()
        {
            BlindingStrike.Configure();
            CombatManeuver.Configure();
            CriticalStrike.Configure();
            Outmanuever.Configure();
            PiercingStrike.Configure();
            SloppyDefense.Configure();
            SurpriseStrike.Configure();

            FeatureSelectionConfigurator.New(FeatName, Guids.DazzlingFeint)
                .SetDisplayName(DisplayName)
                .SetDescription(Description)
                .SetIcon(IconLoader.GetOr(FeatName, AbilityRefs.Blindness.Reference.Get().Icon))
                .SetIsClassFeature()
                .AddToAllFeatures(
                    Guids.BlindingStrike, Guids.CombatManeuver, Guids.CriticalStrike,
                    Guids.Outmanuever, Guids.PiercingStrike, Guids.SloppyDefense,
                    Guids.SurpriseStrike)
                .Configure();
        }
    }
}
