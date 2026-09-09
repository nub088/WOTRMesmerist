using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.References;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Mesmerist.Utils;

namespace Mesmerist.Class.Mesmerist.Archetypes.VexingDaredevil
{
    /// <summary>
    /// Bonus feat granted at 3rd level, replacing Touch Treatment. Tabletop: "You can make a
    /// Bluff check to feint in combat as a move action" (waives the usual Combat Expertise/Int
    /// 13 prerequisites when granted this way).
    ///
    /// Deviation: the engine has no exposed "feint action economy" hook to shorten, so this is
    /// approximated as a flat competence bonus to feint (Persuasion) checks instead.
    /// </summary>
    internal class ImprovedFeint
    {
        private static readonly string FeatName = "ImprovedFeint";
        internal const string DisplayName = "ImprovedFeint.Name";
        private static readonly string Description = "ImprovedFeint.Description";

        public static void Configure()
        {
            FeatureConfigurator.New(FeatName, Guids.ImprovedFeint)
                .SetDisplayName(DisplayName)
                .SetDescription(Description)
                .SetIcon(IconLoader.GetOr(FeatName, AbilityRefs.TrueSeeing.Reference.Get().Icon))
                .SetIsClassFeature()
                .AddStatBonus(ModifierDescriptor.Competence, false, StatType.SkillPersuasion, 4)
                .Configure();
        }
    }
}
