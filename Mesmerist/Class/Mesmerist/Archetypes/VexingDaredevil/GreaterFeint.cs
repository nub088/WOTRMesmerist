using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.References;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Mesmerist.Utils;

namespace Mesmerist.Class.Mesmerist.Archetypes.VexingDaredevil
{
    /// <summary>
    /// Bonus feat granted at 6th level, replacing Touch Treatment's moderate tier. Tabletop:
    /// a successful feint denies the target's Dexterity bonus until the start of your next
    /// turn, not just against your next attack.
    ///
    /// Deviation: same action-economy limitation as Improved Feint - approximated as a further
    /// competence bonus to feint (Persuasion) checks, plus a small edge on the follow-up strike.
    /// </summary>
    internal class GreaterFeint
    {
        private static readonly string FeatName = "GreaterFeint";
        internal const string DisplayName = "GreaterFeint.Name";
        private static readonly string Description = "GreaterFeint.Description";

        public static void Configure()
        {
            FeatureConfigurator.New(FeatName, Guids.GreaterFeint)
                .SetDisplayName(DisplayName)
                .SetDescription(Description)
                .SetIcon(IconLoader.GetOr(FeatName, AbilityRefs.TrueSeeing.Reference.Get().Icon))
                .SetIsClassFeature()
                .AddStatBonus(ModifierDescriptor.Competence, false, StatType.SkillPersuasion, 2)
                .AddStatBonus(ModifierDescriptor.UntypedStackable, false, StatType.AdditionalAttackBonus, 1)
                .Configure();
        }
    }
}
