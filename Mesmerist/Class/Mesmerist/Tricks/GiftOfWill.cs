using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils.Types;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Mesmerist.Utils;

namespace Mesmerist.Class.Mesmerist.Tricks
{
    // LAYER 3 — experimental
    internal class GiftOfWill
    {
        public static void Configure()
        {
            CommonTrickHelpers.CreateTrick("GiftOfWill",
                                           "GiftOfWill.Name",
                                           "GiftOfWill.Description",
                                           AbilityRefs.GoodHope.Reference.Get().Icon,
                                           Guids.GiftOfWill,
                                           Guids.GiftOfWillAbility,
                                           Guids.GiftOfWillBuff);

            BuffConfigurator.For(Guids.GiftOfWillBuff)
                .AddContextStatBonus(StatType.SaveWill, ContextValues.Rank(), ModifierDescriptor.Competence)
                .AddContextRankConfig(ContextRankConfigs.ClassLevel([Guids.Mesmerist], type: AbilityRankType.Default).WithCustomProgression((4, 2), (9, 3), (14, 4), (19, 5), (20, 6)))
                .Configure();
        }
    }
}
