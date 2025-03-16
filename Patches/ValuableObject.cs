using HarmonyLib;

namespace GlassCannon.Patches
{
    [HarmonyPatch(typeof(ValuableObject))]
    public static class ValuableObjectPatches
    {
        [HarmonyPatch("DollarValueSetLogic")]
        [HarmonyPostfix]
        static void DollarValueSetLogicPostfix(ValuableObject __instance)
        {
            // Skip if the object is a SurplusValuable (tax return on extraction)
            if (__instance.GetComponentInParent<SurplusValuable>() != null) return;
            
            Settings.Logger.LogDebug($"DollarValueSetLogicPostfix called. They want their {__instance.dollarValueOriginal} valuable back.");
            
            __instance.dollarValueOriginal *= Settings.DollarMultiplier.Value;
            __instance.dollarValueCurrent = __instance.dollarValueOriginal;
        }
    }
}