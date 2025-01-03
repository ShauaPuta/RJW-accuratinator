using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using rjw;
using Verse;
using Verse.AI;

namespace Lewd_Code
{
    [StaticConstructorOnStartup]
    public static class AddLewdPartPreferences
    {
        private static bool HasGenitalPartProp(Pawn pawn, string partProp)
        {
            var genitals = pawn.GetGenitalsList().OfType<ISexPartHediff>();

            foreach (var genital in genitals)
            {
                HediffDef_SexPart def = genital.Def;

                if (!def.partTags.NullOrEmpty())
                {
                    if (def.partTags.Contains(partProp))
                    {
                        return true;
                    }
                }
            }
            return false;
        }


        /*      private static Func<SexProps, bool> SatisfiesSizeQueen = (SexProps props) =>
             {
                 // get who is penetrating who
                 if (!Lewd_Helper.GetPenetrator(props, out Pawn penetrator, out Pawn recipient))
                 {
                     return false;
                 }
                 //check pp size
                 var adjustedPenetratorDimensions = Lewd_Helper.GetGenitalDimensions(penetrator, Lewd_Helper.TryGetPawnPenisHediff(penetrator));
                 if (adjustedPenetratorDimensions.EnumerableNullOrEmpty())
                 {
                     return false;
                 }
                 //check if holes are used
                 var pawnOrifices = new List<Hediff>();
                 if (props.sexType == xxx.rjwSextype.Anal || props.sexType == xxx.rjwSextype.Vaginal || props.sexType == xxx.rjwSextype.DoublePenetration)
                 {
                     pawnOrifices.Add(Lewd_Helper.TryGetPawnHoleHediff(recipient));
                 }
                 if (pawnOrifices.EnumerableNullOrEmpty())
                 {
                     return false;
                 }
                 //check hole size
                 var adjustedOrificeDimensions = Lewd_Helper.GetGenitalDimensions(recipient, pawnOrifices);
                 if (adjustedOrificeDimensions.EnumerableNullOrEmpty())
                 {
                     return false;
                 }

                 var targetMappings = Lewd_Helper.MapOrificesToPenetrators(adjustedPenetratorDimensions.Keys, pawnOrifices);
                 foreach (var mapping in targetMappings)
                 {
                     var orifice = mapping.Key;

                     // Collect the penetrator dimensions.
                     var penetrators = mapping.Value;
                     var penetratorDimensions = Lewd_Helper.CalculateTotalPenetratorDimensions(penetrators, adjustedPenetratorDimensions);
                     var orificeDimensions = adjustedOrificeDimensions.TryGetValue(orifice);
                     if (penetratorDimensions.Item1 >= orificeDimensions.Item1)
                     {
                         return true;
                     }
                     return false;
                 }

                 return false;
             }; */



        private static Func<SexProps, bool> SatisfiesSizeQueen = (SexProps props) =>
       {
           if (props.partner == null)
           {
               return false;
           }
           IEnumerable<Hediff> penises = props.partner.GetGenitalsList().Where(genitalHediff => Genital_Helper.is_penis(genitalHediff));
           bool hasPenis = !penises.EnumerableNullOrEmpty();

           if (hasPenis)
           {
               foreach (Hediff PPsize in penises)
               {
                   PartSizeCalculator.TryGetLength(PPsize, out float penislength);
                   PartSizeCalculator.TryGetGirth(PPsize, out float penisgirth);
                   float totalpenissize = penisgirth + penislength;
                   IEnumerable<Hediff> vaginas = props.pawn.GetGenitalsList().Where(genitalHediff => Genital_Helper.is_vagina(genitalHediff));
                   bool hasVagina = !vaginas.EnumerableNullOrEmpty();
                   if (hasVagina)
                   {
                       foreach (Hediff VagSize in vaginas)
                       {
                           PartSizeCalculator.TryGetLength(VagSize, out float vaginalength);
                           PartSizeCalculator.TryGetGirth(VagSize, out float vaginagirth);
                           float totalvaginasize = vaginagirth + vaginalength;
                           if (totalvaginasize <= totalpenissize)
                           {
                               Log.Message("total penis size is: " + totalpenissize + ", while total vagina size is: " + totalvaginasize + ", pawn is satisfied because penis is larger than vagina");
                               return true;
                           }
                       }
                   }
                   else
                   {
                       float BodySizePawn = props.pawn?.RaceProps?.baseBodySize ?? 1.0f;
                       float AverageHeight = 170 * BodySizePawn; //average human height of 1,7m multiplied by race body size
                       if (AverageHeight / 5 <= totalpenissize) // if total penis size is at least 1/5 of body size;~35cm for a human; then it satisfies
                       {
                           Log.Message("total penis size is: " + totalpenissize + ", while pawn body height is: " + AverageHeight + ", pawn is satisfied cause pp is friggin huge");
                           return true;
                       }
                   }
               }
           }
           return false;
       };

            

            //var x = pawn.GetGenitalsList().(y => y.ispenis()).firstordefault()
            //if x != null
            //x.TryGetComp<CompHediffBodyPart>();

            

           
            /*var sizeOfallPPCheck = Lewd_Helper.GetGenitalDimensions(props.partner, Lewd_Helper.TryGetPawnPenisHediff(props.partner));
            var sizeOFallHoleCheck = Lewd_Helper.GetGenitalDimensions(props.pawn, Lewd_Helper.TryGetPawnHoleHediff(props.pawn));

            var targetMappings = Lewd_Helper.MapOrificesToPenetrators(sizeOfallPPCheck.Keys, pawnOrifices);
            foreach (var mapping in targetMappings)
            {
                var orifice = mapping.Key;

                // Collect the penetrator dimensions.
                var penetrators = mapping.Value;
                var penetratorDimensions = Lewd_Helper.CalculateTotalPenetratorDimensions(penetrators, adjustedPenetratorDimensions);
                var orificeDimensions = adjustedOrificeDimensions.TryGetValue(orifice);
                if (penetratorDimensions.Item1 >= orificeDimensions.Item1)
                {
                    return true;
                }
                return false;
            }*/

        private static Func<SexProps, bool> SatisfiesKnotLovers = (SexProps props) =>
        {
            if (props.partner == null)
            {
                return false;
            }
            if (!HasGenitalPartProp(props.partner, "Knotted"))
            {
                return false;
            }
            return true;
        };

       private static Func<SexProps, bool> SatisfiesFlareLovers = (SexProps props) =>
        {
            if (props.partner == null)
            {
                return false;
            }
            if (!HasGenitalPartProp(props.partner, "Flared"))
            {
                return false;
            }
            return true;
        };

        private static Func<SexProps, bool> SatisfiesSheathLovers = (SexProps props) =>
        {
            if (props.partner == null)
            {
                return false;
            }
            if (!HasGenitalPartProp(props.partner, "Sheathed"))
            {
                return false;
            }
            return true;
        };

       /* private static Func<SexProps, bool> SatisfiesLongLovers = (SexProps props) =>
        {
            if (props.partner == null)
            {
                return false;
            }
            if (!HasGenitalPartProp(props.partner, "Long"))
            {
                return false;
            }
            return true;
        };
        
        private static Func<SexProps, bool> SatisfiesGirthLovers = (SexProps props) =>
        {
            if (props.partner == null)
            {
                return false;
            }
            if (!HasGenitalPartProp(props.partner, "Girthy"))
            {
                return false;
            }
            return true;
        };*/
         
        private static Func<SexProps, bool> SatisfiesBarbsLovers = (SexProps props) =>
        {
            if (props.partner == null)
            {
                return false;
            }
            if (!HasGenitalPartProp(props.partner, "Barbed"))
            {
                return false;
            }
            return true;
        };

        private static Func<SexProps, bool> SatisfiesTaperLovers = (SexProps props) =>
        {
            if (props.partner == null)
            {
                return false;
            }
            if (!HasGenitalPartProp(props.partner, "Tappered"))
            {
                return false;
            }
            return true;
        };

        private static Func<SexProps, bool> SatisfiesRidgeLovers = (SexProps props) =>
        {
            if (props.partner == null)
            {
                return false;
            }
            if (!HasGenitalPartProp(props.partner, "Ridged"))
            {
                return false;
            }
            return true;
        };

        private static Func<SexProps, bool> SatisfiesInternalLovers = (SexProps props) =>
        {
            if (props.partner == null)
            {
                return false;
            }
            if (!HasGenitalPartProp(props.partner, "Internal"))
            {
                return false;
            }
            return true;
        };

       /* private static Func<SexProps, bool> SatisfiesThinLovers = (SexProps props) =>
        {
            if (props.partner == null)
            {
                return false;
            }
            if (!HasGenitalPartProp(props.partner, "Thin"))
            {
                return false;
            }
            return true;
        };*/

        private static Func<SexProps, bool> SatisfiesRigidLovers = (SexProps props) =>
        {
            if (props.partner == null)
            {
                return false;
            }
            if (!HasGenitalPartProp(props.partner, "Rigid"))
            {
                return false;
            }
            return true;
        };

        private static Func<SexProps, bool> SatisfiesMultiLovers = (SexProps props) =>
        {
            if (props.partner == null)
            {
                return false;
            }
            if (!HasGenitalPartProp(props.partner, "Multiple"))
            {
                return false;
            }
            return true;
        };

       /* private static Func<SexProps, bool> SatisfiesSmallLovers = (SexProps props) =>
        {
            if (props.partner == null)
            {
                return false;
            }
            if (!HasGenitalPartProp(props.partner, "Small"))
            {
                return false;
            }
            return true;
        }; */

        private static Func<SexProps, bool> SatisfiesTentacleLovers = (SexProps props) =>
        {
            if (props.partner == null)
            {
                return false;
            }
            if (!HasGenitalPartProp(props.partner, "Prehensile"))
            {
                return false;
            }
            return true;
        };

        private static Func<SexProps, bool> SatisfiesResizeLovers = (SexProps props) =>
        {
            if (props.partner == null)
            {
                return false;
            }
            if (!HasGenitalPartProp(props.partner, "Resizable"))
            {
                return false;
            }
            return true;
        };

        private static Func<SexProps, bool> SatisfiesHumanLovers = (SexProps props) =>
        {
            if (props.partner == null)
            {
                return false;
            }
            if (!HasGenitalPartProp(props.partner, "Humanlike"))
            {
                return false;
            }
            return true;
        };

        private static Func<SexProps, bool> SatisfiesArtificalLovers = (SexProps props) =>
        {
            if (props.partner == null)
            {
                return false;
            }
            if (!HasGenitalPartProp(props.partner, "Artificial"))
            {
                return false;
            }
            return true;
        };

        private static Func<SexProps, bool> SatisfiesSolidLovers = (SexProps props) =>
        {
            if (props.partner == null)
            {
                return false;
            }
            if (!HasGenitalPartProp(props.partner, "Solid"))
            {
                return false;
            }
            return true;
        };

        private static Func<SexProps, bool> SatisfiesGlowLovers = (SexProps props) =>
        {
            if (props.partner == null)
            {
                return false;
            }
            if (props.partner.def.defName == "GlowingGhoul" ||
            props.partner.def.defName == "Alien_ProtogenNME" || props.partner.def.defName == "Alien_Protogen" ||
            props.partner.def.defName == "ChjAndroid")
            {
                return true;
            }
            if (!HasGenitalPartProp(props.partner, "Glowing"))
            {
                return false;
            }
            return true;
        };

        private static Func<SexProps, bool> SatisfiesMuscleLovers = (SexProps props) =>
        {
            if (props.partner == null)
            {
                return false;
            }
            if (props.partner.story?.bodyType?.defName != "Hulk")
            {
                return false;
            }
            return true;
        };

        private static Func<SexProps, bool> SatisfiesThinLovers = (SexProps props) =>
        {
            if (props.partner == null)
            {
                return false;
            }
            if (props.partner.story?.bodyType?.defName != "Thin")
            {
                return false;
            }
            return true;
        };

        private static Func<SexProps, bool> SatisfiesFatLovers = (SexProps props) =>
        {
            if (props.partner == null)
            {
                return false;
            }
            if (props.partner.story?.bodyType?.defName != "Fat")
            {
                return false;
            }
            return true;
        };

        /* private static Func<SexProps, bool> SatisfiesTigthLovers = (SexProps props) =>
         {
             if (props.partner == null)
             {
                 return false;
             }
             if (!HasGenitalPartProp(props.partner, "Tight"))
             {
                 return false;
             }
             return true;
         };

         private static Func<SexProps, bool> SatisfiesLooseLovers = (SexProps props) =>
         {
             if (props.partner == null)
             {
                 return false;
             }
             if (!HasGenitalPartProp(props.partner, "Loose"))
             {
                 return false;
             }
             return true;
         };

         private static Func<SexProps, bool> SatisfiesDeepLovers = (SexProps props) =>
         {
             if (props.partner == null)
             {
                 return false;
             }
             if (!HasGenitalPartProp(props.partner, "Deep"))
             {
                 return false;
             }
             return true;
         }; */

        public static rjw.Quirk sizeQuirk = new rjw.Quirk("Size Queen", "SizeQueenQuirk", null, SatisfiesSizeQueen);

        public static rjw.Quirk knotQuirk = new rjw.Quirk("Knot lover", "KnotLoverQuirk", null, SatisfiesKnotLovers);
        public static rjw.Quirk flareQuirk = new rjw.Quirk("Flare lover", "FlareLoverQuirk", null, SatisfiesFlareLovers);
        public static rjw.Quirk sheathQuirk = new rjw.Quirk("Sheath player", "SheathLoverQuirk", null, SatisfiesSheathLovers);
    //    public static rjw.Quirk longQuirk = new rjw.Quirk("Long stroker", "{pawn} has a thing for penises that are just longer on the average. A long humanoid dick is just a long dick, but a horse's polearm is a thing {pawn} would love to worship every inch of.", null, SatisfiesLongLovers);
    //    public static rjw.Quirk girthQuirk = new rjw.Quirk("Size queen", "{pawn} really gets it going with cocks that are already girthy on average. A thick human dick has nothing on an average dragon dick in {pawn}'s eyes.", null, SatisfiesGirthLovers);
        public static rjw.Quirk barbsQuirk = new rjw.Quirk("Barbs lover", "BarbsLoverQuirk", null, SatisfiesBarbsLovers);
        public static rjw.Quirk taperQuirk = new rjw.Quirk("Pointy lover", "PointyLoverQuirk", null, SatisfiesTaperLovers);
        public static rjw.Quirk ridgeQuirk = new rjw.Quirk("Texture lover", "RidgeLoverQuirk", null, SatisfiesRidgeLovers);
        public static rjw.Quirk internalQuirk = new rjw.Quirk("Slit lover", "SlitLoverQuirk", null, SatisfiesInternalLovers);
    //    public static rjw.Quirk thinQuirk = new rjw.Quirk("Knot lover", "{pawn} enjoys a penis that has a large bulge somewhere in it. And the though of it getting stuck inside because of it makes {pawn_objective} go nuts.", null, SatisfiesKnotLovers);
        public static rjw.Quirk rigidQuirk = new rjw.Quirk("Hard Lover", "RigidLoverQuirk", null, SatisfiesRigidLovers);
        public static rjw.Quirk multiQuirk = new rjw.Quirk("Multipenis lover", "HemiLoverQuirk", null, SatisfiesMultiLovers);
    //    public static rjw.Quirk smallQuirk = new rjw.Quirk("Knot lover", "{pawn} enjoys a penis that has a large bulge somewhere in it. And the though of it getting stuck inside because of it makes {pawn_objective} go nuts.", null, SatisfiesKnotLovers);
        public static rjw.Quirk tentacleQuirk = new rjw.Quirk("Tentacle lover", "TentacleLoverQuirk", null, SatisfiesTentacleLovers);
        public static rjw.Quirk resizeQuirk = new rjw.Quirk("Fickle", "FickleQuirk", null, SatisfiesResizeLovers);
        public static rjw.Quirk humanQuirk = new rjw.Quirk("Basic", "HumanLoverQuirk", null, SatisfiesHumanLovers);
        public static rjw.Quirk manmadeQuirk = new rjw.Quirk("Genital prostophile", "ArtificialLoverQuirk", null, SatisfiesArtificalLovers);
        public static rjw.Quirk solidQuirk = new rjw.Quirk("Solid Lover", "SolidLoverFetish", null, SatisfiesSolidLovers);
        public static rjw.Quirk glowQuirk = new rjw.Quirk("Moth person", "GlowLoverQuirk", null, SatisfiesGlowLovers);
        public static rjw.Quirk fatQuirk = new rjw.Quirk("Chubby lover", "FatLoverQuirk", null, SatisfiesFatLovers);
        public static rjw.Quirk muscleQuirk = new rjw.Quirk("Abs licker", "MuscleLoverQuirk", null, SatisfiesMuscleLovers);
        public static rjw.Quirk thinQuirk = new rjw.Quirk("Skinny lover", "ThinLoverQuirk", null, SatisfiesThinLovers);

        //public static readonly Quirk muscleQuirk = new Quirk(
        //    "Abs licker",
        //    "MuscleLoverQuirk",
        //    (pawn, partner) =>
        //        partner.story.bodyType.label == "Hulk"
        //    );

        //public static readonly Quirk fatQuirk = new Quirk(
        //    "Fat lover",
        //    "FatLoverQuirk",
        //    (pawn, partner) =>
        //        partner.story.bodyType.label == "Fat"
        //    );

        //public static readonly Quirk thinQuirk = new Quirk(
        //    "Skinny lover",
        //    "ThinLoverQuirk",
        //    (pawn, partner) =>
        //        partner.story.bodyType.label == "Thin"
        //    );
        //    public static rjw.Quirk tightQuirk = new rjw.Quirk("Knot lover", "{pawn} enjoys a penis that has a large bulge somewhere in it. And the though of it getting stuck inside because of it makes {pawn_objective} go nuts.", null, SatisfiesKnotLovers);
        //    public static rjw.Quirk looseQuirk = new rjw.Quirk("Knot lover", "{pawn} enjoys a penis that has a large bulge somewhere in it. And the though of it getting stuck inside because of it makes {pawn_objective} go nuts.", null, SatisfiesKnotLovers);
        //    public static rjw.Quirk deepQuirk = new rjw.Quirk("Knot lover", "{pawn} enjoys a penis that has a large bulge somewhere in it. And the though of it getting stuck inside because of it makes {pawn_objective} go nuts.", null, SatisfiesKnotLovers);
    }
}