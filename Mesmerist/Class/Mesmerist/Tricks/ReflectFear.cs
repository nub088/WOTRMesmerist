using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints.Classes.Spells;
using Mesmerist.Utils;

namespace Mesmerist.Class.Mesmerist.Tricks
{
    internal class ReflectFear
    {
        public static void Configure()
        {
            CommonTrickHelpers.CreateTrick("ReflectFear",
                                           "ReflectFear.Name",
                                           "ReflectFear.Description",
                                           AbilityRefs.Eyebite.Reference.Get().Icon,
                                           Guids.ReflectFear,
                                           Guids.ReflectFearAbility,
                                           Guids.ReflectFearBuff);

            // CANDIDATE: AddBuffDescriptorImmunity. If unavailable, fall back to
            // .AddFacts(new() { BuffRefs.RemoveFearBuff.Reference.Get() }). // TODO(verify-on-build)
            BuffConfigurator.For(Guids.ReflectFearBuff)
                .AddBuffDescriptorImmunity(descriptor: SpellDescriptor.Fear)
                .Configure();
        }
    }
}
