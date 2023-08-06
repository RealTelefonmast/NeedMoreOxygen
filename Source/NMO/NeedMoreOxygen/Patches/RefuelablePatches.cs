using HarmonyLib;
using RimWorld;
using TeleCore;
using Verse;

namespace NMO.Patches;

internal static class RefuelablePatches
{
    [HarmonyPatch(typeof(CompRefuelable), nameof(CompRefuelable.HasFuel))]
    private static class CompRefuelable_HasFuel_Patch
    {
        private static void Postfix(CompRefuelable __instance, ref bool __result)
        {
            if (__result) return;
            if (__instance.parent is not ThingWithComps thing) return;
            if (OxygenUtility.IsBurner(thing))
            {
                //__result &= thing.Map.GetMapInfo<AirMapInfo>().HasOxygen(thing);
            }
        }
    }
}