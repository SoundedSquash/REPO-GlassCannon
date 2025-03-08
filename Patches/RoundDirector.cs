using System;
using HarmonyLib;
using Photon.Pun;

namespace GlassCannon.Patches
{
    [HarmonyPatch(typeof(RoundDirector))]
    public static class RoundDirectorPatches
    {
        private static bool _valueSet;
        private static Level? _currentLevel = RunManager.instance?.levelMainMenu;
        
        [HarmonyPatch("StartRoundLogic")]
        [HarmonyPrefix]
        // This runs before the round is started.
        public static void StartRoundLogicPrefix(RoundDirector __instance, ref int value)
        {
            // Only for host.
            if (GameManager.instance.gameMode != 0 && !PhotonNetwork.IsMasterClient) return;

            // Reset valueSet if level changes.
            if (_currentLevel != RunManager.instance.levelCurrent) _valueSet = false;
            
            // Only run once.
            if (_valueSet)
            {
                return;
            }
            
            Settings.Logger.LogDebug($"StartRoundLogicPostfix({value}) In");
            // Unapply multiplication and round down.
            value = (int)Math.Floor(value / Settings.DollarMultiplier.Value);
            _valueSet = true;
            _currentLevel = RunManager.instance.levelCurrent;

            Settings.Logger.LogDebug($"StartRoundLogicPostfix({value}) Out");
        }
    }
}