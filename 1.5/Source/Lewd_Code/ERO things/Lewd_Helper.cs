using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using rjw;
using rjw.Modules.Interactions.Objects;
using rjw.Modules.Interactions.Helpers;
using rjw.Modules.Interactions.Enums;
using Verse;
using Verse.AI;

namespace Lewd_Code
{
    /*class Lewd_Helper // Totally did not give up and copy pasted licentia, no no of course no
    {
        public static Dictionary<Hediff, (float, float)> GetGenitalDimensions(Pawn pawn, IEnumerable<Hediff> hediffs, float defaultValue = float.MinValue)
        {
            Dictionary<Hediff, (float, float)> hediffDimensions = new Dictionary<Hediff, (float, float)>();

            // Calculate the length and girth of each Hediff.
            foreach (var hediff in hediffs)
            {
                CompHediffBodyPart rjwHediff = hediff.TryGetComp<CompHediffBodyPart>();
                if (rjwHediff != null)
                {
                    if (!PartSizeExtension.TryGetLength(hediff, out float length)) length = defaultValue == float.MinValue ? hediff.Severity : defaultValue;

                    if (!PartSizeExtension.TryGetGirth(hediff, out float girth)) girth = defaultValue == float.MinValue ? hediff.Severity : defaultValue;

                    if (length < 0 || girth < 0)
                    {
                        continue;
                    }
                    hediffDimensions.Add(hediff, (length, girth));
                }
                else
                {
                    if (hediff.Severity < 0)
                    {
                        continue;
                    }
                    hediffDimensions.Add(hediff, (hediff.Severity, hediff.Severity));
                }
            }

            return hediffDimensions;
        }

        public static bool GetPenetrator(SexProps props, out Pawn penetrator, out Pawn penetrated)
        {
            penetrator = null;
            penetrated = null;

            bool initiatorHasPenis = true;
            bool partnerHasPenis = true;
            if (TryGetPawnPenisHediff(props.pawn).EnumerableNullOrEmpty())
            {
                initiatorHasPenis = false;
            }
            if (TryGetPawnPenisHediff(props.partner).EnumerableNullOrEmpty())
            {
                partnerHasPenis = false;
            }

            if (!initiatorHasPenis && !partnerHasPenis)
            {
                return false;
            }

            InteractionWithExtension interaction = InteractionHelper.GetWithExtension(props.dictionaryKey);
            if (initiatorHasPenis && (interaction?.HasInteractionTag(InteractionTag.Reverse)).GetValueOrDefault(false))
            {
                penetrator = props.pawn;
                penetrated = props.partner;
                return true;
            }
            else if (partnerHasPenis)
            {
                penetrator = props.partner;
                penetrated = props.pawn;
                return true;
            }

            return false;
        }

        public static List<Hediff> TryGetPawnPenisHediff(Pawn pawn)
        {
            var hediffs = new HashSet<Hediff>();

            var genitalsHediffs = Genital_Helper.get_PartsHediffList(pawn, Genital_Helper.get_genitalsBPR(pawn));
            if (Genital_Helper.has_penis_fertile(pawn, genitalsHediffs) || Genital_Helper.has_penis_infertile(pawn, genitalsHediffs) || Genital_Helper.has_multipenis(pawn, genitalsHediffs))
            {
                hediffs.AddRange(genitalsHediffs.FindAll((Hediff hediff) => hediff.def.defName.ToLower().Contains("penis")));
                hediffs.AddRange(genitalsHediffs.FindAll((Hediff hediff) => hediff.def.defName.ToLower().Contains("ovipositorf")));
                hediffs.AddRange(genitalsHediffs.FindAll((Hediff hediff) => hediff.def.defName.ToLower().Contains("ovipositorm")));
                hediffs.AddRange(genitalsHediffs.FindAll((Hediff hediff) => hediff.def.defName.ToLower().Contains("tentacle")));
            }
            else if (Genital_Helper.has_ovipositorF(pawn, genitalsHediffs))
            {
                hediffs.AddRange(genitalsHediffs.FindAll((Hediff hediff) => hediff.def.defName.ToLower().Contains("ovipositorf")));
            }

            return hediffs.ToList();
        }

        public static List<Hediff> TryGetPawnHoleHediff(Pawn pawn) // used for stretch and futa calculation
        {
            var hediffs = new HashSet<Hediff>();

            var genitalsHediffs = Genital_Helper.get_PartsHediffList(pawn, Genital_Helper.get_genitalsBPR(pawn));
            if (Genital_Helper.has_vagina(pawn, genitalsHediffs) || Genital_Helper.has_anus(pawn, genitalsHediffs) || Genital_Helper.has_ovipositorF(pawn, genitalsHediffs))
            {
                hediffs.AddRange(genitalsHediffs.FindAll((Hediff hediff) => hediff.def.defName.ToLower().Contains("vagina")));
                hediffs.AddRange(genitalsHediffs.FindAll((Hediff hediff) => hediff.def.defName.ToLower().Contains("ovipositorf")));
                hediffs.AddRange(genitalsHediffs.FindAll((Hediff hediff) => hediff.def.defName.ToLower().Contains("anus")));
            }
            else if (Genital_Helper.has_ovipositorF(pawn, genitalsHediffs))
            {
                hediffs.AddRange(genitalsHediffs.FindAll((Hediff hediff) => hediff.def.defName.ToLower().Contains("ovipositorf")));
            }

            return hediffs.ToList();
        }

        /*public static Hediff TryGetPawnHoleHediff(Pawn pawn)
        {
            Hediff hediff = null;
            var pawnparts = Genital_Helper.get_PartsHediffList(pawn, Genital_Helper.get_genitalsBPR(pawn));

            if (Genital_Helper.has_vagina(pawn, pawnparts))
            {
                hediff = pawnparts.FindAll((Hediff hed) => hed.def.defName.ToLower().Contains("vagina")).InRandomOrder().FirstOrDefault();

                if (hediff == null)
                    hediff = pawnparts.FindAll((Hediff hed) => hed.def.defName.ToLower().Contains("ovipositorf")).InRandomOrder().FirstOrDefault();
            }
            if (Genital_Helper.has_anus(pawn, pawnparts))
            {
                hediff = pawnparts.FindAll((Hediff hed) => hed.def.defName.ToLower().Contains("anus")).InRandomOrder().FirstOrDefault();
            }
            else
            {
                hediff = null;
            }

            return hediff;
        }*/

      /*  public static Dictionary<List<Hediff>, List<Hediff>> MapOrificesToPenetrators(IEnumerable<Hediff> penetrators, IEnumerable<Hediff> orifices)
        {
            var penetratorList = penetrators.ToList();
            penetratorList.Shuffle();
            var orificeList = orifices.ToList();
            var n0orifices = orificeList.Count;

            var output = new Dictionary<List<Hediff>, List<Hediff>>();
            for (int ii = 0; ii < n0orifices; ii++)
            {
                if (!output.TryGetValue(orificeList, out List<Hediff> mappedPenetrators))
                {
                    mappedPenetrators = new List<Hediff>();
                    output.Add(orificeList, mappedPenetrators);
                }
                var penetrator = penetratorList[ii];
                mappedPenetrators.Add(penetrator);
            }

            return output;
            /* var nOrifices = orifices.Count();
             for (int ii = 0; ii < penetratorList.Count(); ++ii)
             {
                 var orifice = orifices[ii % nOrifices];
                 if (!output.TryGetValue(orifice, out List<Hediff> mappedPenetrators))
                 {
                     mappedPenetrators = new List<Hediff>();
                     output.Add(orifice, mappedPenetrators);
                 }
                 var penetrator = penetratorList[ii];
                 mappedPenetrators.Add(penetrator);*/
        }

       /* public static (float, float) CalculateTotalPenetratorDimensions(IEnumerable<Hediff> penetrators, Dictionary<Hediff, (float, float)> penetratorDimensions)
        {
            var length = 0f;
            var totalGirth = 0f;
            foreach (var penetrator in penetrators)
            {
                var dimensions = penetratorDimensions.TryGetValue(penetrator);
                if (length == 0f || totalGirth == 0f || true/*Rand.Range(0f, 1f) > 0.5f)
                {
                    length = Math.Max(length, dimensions.Item1);
                    totalGirth += dimensions.Item2;
                }
            }
            return (length, totalGirth);
        }
    }
}*/
