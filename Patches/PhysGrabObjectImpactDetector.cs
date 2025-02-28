using HarmonyLib;
using UnityEngine;

namespace GlassCannon.Patches
{
    [HarmonyPatch(typeof(PhysGrabObjectImpactDetector))]
    public static class PhysGrabObjectImpactDetectorPatches
    {
        [HarmonyPatch("Break")]
        [HarmonyPrefix]
        static void BreakPrefix(ref float valueLost, Vector3 _contactPoint, int breakLevel)
        {
            valueLost = Settings.ItemImpactBehavior.Value switch
            {
                1 => float.MaxValue, // Break on impact
                2 => 0f, // No damage
                _ => valueLost // Default
            };
            Settings.Logger.LogDebug($"Break detected. {valueLost} value lost. [{Settings.ItemImpactBehavior.Value}]");
        }
    }
}