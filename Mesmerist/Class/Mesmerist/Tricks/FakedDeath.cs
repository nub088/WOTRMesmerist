using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils.Types;
using Mesmerist.Utils;

namespace Mesmerist.Class.Mesmerist.Tricks
{
    // LAYER 3 — experimental
    internal class FakedDeath
    {
        public static void Configure()
        {
            CommonTrickHelpers.CreateMasterfulTrick("FakedDeath",
                                                    "FakedDeath.Name",
                                                    "FakedDeath.Description",
                                                    AbilityRefs.Displacement.Reference.Get().Icon,
                                                    Guids.FakedDeath,
                                                    Guids.FakedDeathAbility,
                                                    Guids.FakedDeathBuff);

            // doNotRestoreMissingFacts matters here. The vanilla invisibility buff removes
            // itself when its holder takes an offensive action (Kingmaker.Designers.Mechanics.
            // Buffs.BuffInvisibility.HandleUnitMakeOffensiveAction), which is the behaviour we
            // want - but AddFacts.UpdateFacts re-adds any of its facts that have gone missing
            // when a save is loaded, so without this flag a save/load after breaking the
            // invisibility would hand it straight back for the rest of the duration.
            BuffConfigurator.For(Guids.FakedDeathBuff)
                .AddFacts(new() { BuffRefs.InvisibilityBuff.Reference.Get() }, doNotRestoreMissingFacts: true)
                .Configure();
        }
    }
}
