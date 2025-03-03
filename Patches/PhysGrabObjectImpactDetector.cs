using HarmonyLib;
using UnityEngine;

namespace GlassCannon.Patches
{
    [HarmonyPatch(typeof(PhysGrabObjectImpactDetector))]
    public static class PhysGrabObjectImpactDetectorPatches
    {
        [HarmonyPatch("Break")]
        [HarmonyPrefix]
        static void BreakPrefix(ref float valueLost, Vector3 _contactPoint, int breakLevel,  ValuableObject? ___valuableObject)
        {
            if (___valuableObject == null) return;
            
            valueLost = Settings.ItemImpactBehavior.Value switch
            {
                1 => ___valuableObject.dollarValueCurrent, // Break on impact
                2 => 0f, // No damage
                _ => valueLost // Default
            };
            Settings.Logger.LogDebug($"Break detected. {___valuableObject.name} lost {valueLost} value. [{Settings.ItemImpactBehavior.Value}]");
        }
    }
}