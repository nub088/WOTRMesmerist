using BlueprintCore.Blueprints.CustomConfigurators.Classes.Selection;
using BlueprintCore.Blueprints.References;
using Mesmerist.Utils;

namespace Mesmerist.Class.Mesmerist.Tricks
{
    internal class TrickSelection
    {
        private static readonly string FeatName = "MesmeristTricks";
        internal const string DisplayName = "MesmeristTrick.Name";
        private static readonly string Description = "MesmeristTrick.Description";
        public static void Configure()
        {
            AstoundingAvoidance.Configure();
            ConcealingVeil.Configure();
            FalseFlanker.Configure();
            FleetInShadows.Configure();
            FearsomeGuise.Configure();
            ForcedHope.Configure();
            FreeInBody.Configure();
            ReflectFear.Configure();
            ShadowSplinter.Configure();
            SpectralSmoke.Configure();
            LinkedReaction.Configure();
            MeekFacade.Configure();
            MesmericMirror.Configure();
            MesmericPantomime.Configure();
            PsychosomaticSurge.Configure();
            SeeInDarkness.Configure();
            ShadowBlend.Configure();
            SlipBonds.Configure();
            UnwittingMessanger.Configure();
            VanishArrow.Configure();
            VoiceOfReason.Configure();

            // === EXPERIMENTAL (low-confidence / Layer 3 — comment out this block to strip) ===
            CompelAlacrity.Configure();
            LevitationBuffer.Configure();
            Misdirection.Configure();
            CursedSanction.Configure();
            GiftOfWill.Configure();
            UmbralShield.Configure();
            VisionOfBlood.Configure();
            FakedDeath.Configure();
            // === END EXPERIMENTAL ===

            FeatureSelectionConfigurator.New(FeatName + "Selection", Guids.MesmeristTrickSelection)
                .SetDisplayName(DisplayName)
                .SetDescription(Description)
                .SetIcon(FeatureRefs.ArcanistExploits.Reference.Get().Icon)
                .SetIsClassFeature()
                .AddToAllFeatures([Guids.FalseFlanker, Guids.MeekFacade, Guids.MesmericPantomime, Guids.MesmericMirror, Guids.PsychosomaticSurge, Guids.VoiceOfReason,
                Guids.SeeInDarkness, Guids.UnwittingMessanger, Guids.FearsomeGuise, Guids.SlipBonds, Guids.VanishArrow, Guids.FreeInBody, Guids.ShadowBlend, Guids.ConcealingVeil,
                Guids.ForcedHope, Guids.LinkedReaction, Guids.FleetInShadows, Guids.AstoundingAvoidance, Guids.ReflectFear, Guids.ShadowSplinter, Guids.SpectralSmoke,
                // EXPERIMENTAL (strip with the block above):
                Guids.CompelAlacrity, Guids.LevitationBuffer, Guids.Misdirection, Guids.CursedSanction,
                Guids.GiftOfWill, Guids.UmbralShield, Guids.VisionOfBlood, Guids.FakedDeath])
                /*.AddToAllFeatures([Guids.AstoundingAvoidance, Guids.CompelAlacrity, Guids.FalseFlanker,
                 Guids.FleetInShadows, Guids.LevitationBuffer,
                Guids.LinkedReaction, MesmericMirror,
                , Guids.Misdirection,
                Guids.ReflectFear,  Guids.ShadowSplinter,
                Guids.VanishArrow, //Guids.SpectralSmoke,
                Guids.CursedSanction, Guids.FreeInBody, Guids.GoodHopeTrick,
                Guids.ShadowBlend])*/
                .Configure();
        }
    }
}
