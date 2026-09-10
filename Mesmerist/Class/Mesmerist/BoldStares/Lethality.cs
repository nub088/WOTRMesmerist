using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using Mesmerist.Utils;
using Kingmaker.Enums;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints.Classes.Spells;

namespace Mesmerist.Class.Mesmerist.BoldStares
{
    public class Lethality
    {
        private static readonly string FeatName = "Lethality";
        internal const string DisplayName = "Lethality.Name";
        private static readonly string Description = "Lethality.Description";

        public static void Configure()
        {

            BuffConfigurator.New(FeatName + "Buff", Guids.LethalityBuff)
                .SetDisplayName(DisplayName)
                .SetDescription(Description)
                .AddUniqueBuff()
                .SetIcon(IconLoader.GetOr("Lethality", BuffRefs.LethalStanceEffectBuff.Reference.Get().Icon))
                // --- FIX 3: scope the penalty to poison/disease saves (1e RAW) ---------
                // 1e RAW extends the hypnotic stare penalty only to saves against poison
                // and disease, not to Fortitude saves in general (death effects, stunning,
                // etc. are unaffected). AddContextStatBonus(StatType.SaveFortitude, ...)
                // applied the penalty to every Fortitude save the target rolls, so this is
                // conditioned on the incoming effect's descriptor instead. Same rank-scaled
                // magnitude as before (-1 * rank, i.e. -2 baseline, -3 at 19, -5 at 20 per
                // the AddContextRankConfig below).
                //
                // To revert to the previous (blanket Fortitude) behavior, swap this line
                // back to:
                //   .AddContextStatBonus(StatType.SaveFortitude, ContextValues.Rank(), ModifierDescriptor.UntypedStackable, 2, -1)
                // (and restore the `using Kingmaker.EntitySystem.Stats;` import for StatType).
                .AddSavingThrowBonusAgainstDescriptor(modifierDescriptor: ModifierDescriptor.UntypedStackable, spellDescriptor: SpellDescriptor.Poison | SpellDescriptor.Disease, value: -1)
                // --- end FIX 3 -----------------------------------------------------------
                .AddContextRankConfig(ContextRankConfigs.CharacterLevel(AbilityRankType.Default).WithCustomProgression((7, 2), (19, 3), (20, 5)))
                .Configure();

            //TODO: Change CharacterLevel to ClassLevel(Mesmerist)
            FeatureConfigurator.New(FeatName, Guids.Lethality)
                .SetDisplayName(DisplayName)
                .SetDescription(Description)
                .SetIsClassFeature()
                .SetIcon(IconLoader.GetOr("Lethality", AbilityRefs.TrueSeeing.Reference.Get().Icon))
                .Configure();


        }
    }
}
