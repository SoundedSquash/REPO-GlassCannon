using HarmonyLib;

namespace GlassCannon.Patches
{
    [HarmonyPatch(typeof(ValuableObject))]
    public static class ValuableObjectPatches
    {
        [HarmonyPatch("Start")]
        [HarmonyPostfix]
        static void StartPostfix(ValuableObject __instance)
        {
            ValuableObjectManager.AddValuableObject(__instance);
            Settings.Logger.LogDebug($"ValuableObject {__instance.name} added");
        }
    }
}