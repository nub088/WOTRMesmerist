using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.References;
using Mesmerist.Utils;

namespace Mesmerist.Class.Mesmerist.Archetypes.VexingDaredevil
{
    /// <summary>
    /// Vexing Daredevil, 11th level, replaces Glib Tongue (which this mod never implemented, so
    /// there is nothing to remove). Tabletop: "when a vexing daredevil moves in a round, until
    /// the beginning of her next turn, any target of her hypnotic stare sees her as a constantly
    /// shimmering form of light and treats her as if she were under a blur spell."
    ///
    /// Deviation: "only while moving, and only as seen by stared targets" isn't cheaply
    /// expressible, so this grants a permanent 20% miss chance (as Blur) instead.
    /// </summary>
    internal class ShimmeringBody
    {
        private static readonly string FeatName = "ShimmeringBody";
        internal const string DisplayName = "ShimmeringBody.Name";
        private static readonly string Description = "ShimmeringBody.Description";

        public static void Configure()
        {
            FeatureConfigurator.New(FeatName, Guids.ShimmeringBody)
                .SetDisplayName(DisplayName)
                .SetDescription(Description)
                .SetIcon(IconLoader.GetOr(FeatName, AbilityRefs.Displacement.Reference.Get().Icon))
                .SetIsClassFeature()
                .AddFacts(new() { BuffRefs.BlurBuff.Reference.Get() })
                .Configure();
        }
    }
}
