using HarmonyLib;

namespace GlassCannon.Patches
{
    [HarmonyPatch(typeof(RoundDirector))]
    public static class RoundDirectorPatches
    {
        [HarmonyPatch("StartRoundLogic")]
        [HarmonyPostfix]
        public static void StartRoundLogicPostfix()
        {
            Settings.Logger.LogDebug("StartRoundLogicPostfix Started");
            // This runs AFTER the round is started.
            if (ValuableObjectManager.Instance != null)
            {
                ValuableObjectManager.Instance.MultiplyValuableObjectsValue();
            }
            Settings.Logger.LogDebug("StartRoundLogicPostfix Ended");
        }
    }
}