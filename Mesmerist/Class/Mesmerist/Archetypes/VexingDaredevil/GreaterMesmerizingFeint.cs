using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.References;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Mesmerist.Utils;

namespace Mesmerist.Class.Mesmerist.Archetypes.VexingDaredevil
{
    /// <summary>
    /// Bonus feat granted at 10th level, replacing Touch Treatment's greater tier. Tabletop:
    /// lets the daredevil feint against non-humanoid, animal-minded, and (at the greater tier)
    /// mindless creatures as long as they are subjects of her hypnotic stare.
    ///
    /// Deviation: creature-type feint restrictions aren't modeled by the engine, so this is
    /// approximated as a further competence bonus to feint (Persuasion) checks.
    /// </summary>
    internal class GreaterMesmerizingFeint
    {
        private static readonly string FeatName = "GreaterMesmerizingFeint";
        internal const string DisplayName = "GreaterMesmerizingFeint.Name";
        private static readonly string Description = "GreaterMesmerizingFeint.Description";

        public static void Configure()
        {
            FeatureConfigurator.New(FeatName, Guids.GreaterMesmerizingFeint)
                .SetDisplayName(DisplayName)
                .SetDescription(Description)
                .SetIcon(IconLoader.GetOr(FeatName, AbilityRefs.TrueSeeing.Reference.Get().Icon))
                .SetIsClassFeature()
                .AddStatBonus(ModifierDescriptor.Competence, false, StatType.SkillPersuasion, 2)
                .Configure();
        }
    }
}
