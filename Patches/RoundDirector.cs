using HarmonyLib;

namespace GlassCannon.Patches
{
    [HarmonyPatch(typeof(RoundDirector))]
    public static class RoundDirectorPatches
    {
        [HarmonyPatch("StartRoundLogic")]
        [HarmonyPostfix]
        // This runs AFTER the round is started.
        public static void StartRoundLogicPostfix()
        {
            Settings.Logger.LogDebug("StartRoundLogicPostfix Started");
            
            ValuableObjectManager.MultiplyValuableObjectsValue();
            
            Settings.Logger.LogDebug("StartRoundLogicPostfix Ended");
        }
    }
}