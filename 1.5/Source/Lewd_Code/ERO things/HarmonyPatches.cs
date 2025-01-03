using System.Reflection;
using RimWorld;
using Verse;
using HarmonyLib;
using rjw;

namespace Lewd_Code
{
    class HamonyPatches
    {
        [StaticConstructorOnStartup]
        public static class HarmonyPatches
        {
            static HarmonyPatches()
            {
                var harmony = new Harmony("Shauaputa.Accuratinator");
                harmony.PatchAll(Assembly.GetExecutingAssembly());
            }
            [HarmonyPatch(typeof(SexUtility), nameof(SexUtility.SatisfyPersonal))]
            public static class Lewd_QuirkChanges
            {
                [HarmonyPostfix]
                public static void Apply_ThoughyMoreThanOnce(SexProps props, float satisfaction = 0.4f)
                {
                    Pawn pawn = props.pawn;
                    int howMany = Quirk.CountSatisfiedQuirks(props);
                    int thisMany = howMany - 1;
                    if (thisMany > 0)
                    {
                        for (int i = 0; i < thisMany; i++)
                        {
                            Quirk.AddThought(pawn);
                        }
                    }
                }
            }
        }
    }
}
