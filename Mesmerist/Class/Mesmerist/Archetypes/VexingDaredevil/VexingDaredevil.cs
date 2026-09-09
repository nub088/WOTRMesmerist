using BlueprintCore.Blueprints.Configurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils.Types;
using Mesmerist.Utils;

namespace Mesmerist.Class.Mesmerist.Archetypes.VexingDaredevil
{
    /// <summary>
    /// Vexing Daredevil (Pathfinder: Occult Adventures) - a mesmerist archetype built for
    /// melee combat: she trades her tricks' 1st-level pick, touch treatment and bold stare for
    /// weapon training, bonus feint feats and Dazzling Feint, a hypnotic-stare-linked rider on
    /// her attacks.
    ///
    /// Class Skills: tabletop adds Acrobatics. This mod already gives the base mesmerist the
    /// merged Mobility skill (which covers Acrobatics), so there is nothing to add here.
    ///
    /// Run after <see cref="MesmeristClass.Configure"/> - it appends onto HypnoticStareBuff and
    /// removes entries from the base progression, both of which must already exist.
    /// </summary>
    public class VexingDaredevilArchetype
    {
        private static readonly string ArchetypeName = "VexingDaredevilArchetype";
        internal const string DisplayName = "VexingDaredevil.Name";
        private static readonly string Description = "VexingDaredevil.Description";

        public static void Configure()
        {
            ImprovedFeint.Configure();
            GreaterFeint.Configure();
            GreaterMesmerizingFeint.Configure();
            DazzlingFeint.Configure();
            ShimmeringBody.Configure();

            var addFeatures = LevelEntryBuilder.New()
                // Martial Weapon Proficiency (1st) replaces the 1st-level mesmerist trick.
                // Reuses the game's own "pick one martial weapon" selection rather than
                // rebuilding it - see Guids.MartialWeaponProficiencySelection.
                .AddEntry(1, Guids.MartialWeaponProficiencySelection)
                .AddEntry(3, Guids.ImprovedFeint, Guids.DazzlingFeint)
                .AddEntry(6, Guids.GreaterFeint)
                .AddEntry(7, Guids.DazzlingFeint)
                .AddEntry(10, Guids.GreaterMesmerizingFeint)
                .AddEntry(11, Guids.DazzlingFeint, Guids.ShimmeringBody)
                // "Gains a bonus stare feat" at 14th - approximated as an extra bold stare pick,
                // the closest existing analog to a tabletop "stare feat".
                .AddEntry(14, Guids.BoldStareSelection)
                .AddEntry(15, Guids.DazzlingFeint)
                .AddEntry(19, Guids.DazzlingFeint);

            var removeFeatures = LevelEntryBuilder.New()
                .AddEntry(1, Guids.MesmeristTrickSelection, Guids.TouchTreatmentResourceFeature)
                .AddEntry(3, Guids.TouchTreatment, Guids.BoldStareSelection)
                .AddEntry(6, Guids.TouchTreatmentModerate)
                .AddEntry(7, Guids.BoldStareSelection)
                .AddEntry(10, Guids.TouchTreatmentGreater)
                .AddEntry(11, Guids.BoldStareSelection)
                .AddEntry(14, Guids.TouchTreatmentBreak)
                .AddEntry(15, Guids.BoldStareSelection)
                .AddEntry(19, Guids.BoldStareSelection);

            // Passing Guids.Mesmerist here already registers this archetype on the class
            // (ArchetypeConfigurator.OnConfigureCompleted calls
            // CharacterClassConfigurator.For(Clazz).AddToArchetypes(...) itself); a second,
            // explicit AddToArchetypes call here previously duplicated the entry, showing the
            // archetype twice at character creation.
            ArchetypeConfigurator.New(ArchetypeName, Guids.VexingDaredevil, Guids.Mesmerist)
                .SetLocalizedName(DisplayName)
                .SetLocalizedDescription(Description)
                .SetIcon(IconLoader.GetOr("VexingDaredevil", AbilityRefs.EyebiteAbility.Reference.Get().Icon))
                .SetAddFeatures(addFeatures)
                .SetRemoveFeatures(removeFeatures)
                .Configure();
        }
    }
}
