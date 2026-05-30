using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils.Types;
using Kingmaker.Enums;
using Mesmerist.Utils;

namespace Mesmerist.Class.Mesmerist.Tricks
{
    internal class SpectralSmoke
    {
        public static void Configure()
        {
            CommonTrickHelpers.CreateTrick("SpectralSmoke",
                                           "SpectralSmoke.Name",
                                           "SpectralSmoke.Description",
                                           AbilityRefs.Echolocation.Reference.Get().Icon,
                                           Guids.SpectralSmoke,
                                           Guids.SpectralSmokeAbility,
                                           Guids.SpectralSmokeBuff);

            BuffConfigurator.For(Guids.SpectralSmokeBuff)
                .AddConcealment(false, false, Concealment.Partial, ConcealmentDescriptor.Fog)
                .Configure();
        }
    }
}
