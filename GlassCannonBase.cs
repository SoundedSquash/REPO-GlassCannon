using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using UnityEngine;

namespace HardMode
{
    [BepInPlugin(PluginGuid, PluginName, PluginVersion)]
    public class GlassCannonBase : BaseUnityPlugin
    {
        private const string PluginGuid = "soundedsquash.glasscannon";
        private const string PluginName = "Glass Cannon";
        private const string PluginVersion = "1.0.0.0";
        
        private readonly Harmony _harmony = new Harmony(PluginGuid);

        private static ConfigEntry<float> DollarMultiplier;

        private static ConfigEntry<int> ItemImpactBehavior;

        private static readonly ManualLogSource ManualLogSource = BepInEx.Logging.Logger.CreateLogSource(PluginGuid);

        public void Awake()
        {
            DollarMultiplier = Config.Bind(
                "General",
                "DollarMultiplier",
                2f,
                "The multiplier that is applied to item value. 2 doubles the value. 1 is the same. Any number between 0 and 1 will lose value.");
            
            ItemImpactBehavior = Config.Bind(
                "General",
                "ItemImpactBehavior",
                1,
                @"0 - Default impacts.  1 - Break on impact.  2 - No impact damage.");
            
            // Validate config
            if (DollarMultiplier.Value > 0f)
            {
                _harmony.PatchAll(typeof(PhysGrabObjectImpactDetector_Break));
            }
            else
            {
                ManualLogSource.LogError($"Dollar multiplier value of {DollarMultiplier.Value} is invalid.");
                return;
            }
            
            // Validate config
            if (ItemImpactBehavior.Value >= 0 && ItemImpactBehavior.Value <= 2)
            {
                _harmony.PatchAll(typeof(ValuableObject_DollarValueSetLogic));
            }
            else
            {
                ManualLogSource.LogError($"Enter a valid value for Item Impact Behavior. Invalid value: {ItemImpactBehavior.Value}.");
                return;
            }
            
            ManualLogSource.LogInfo($"{PluginName} loaded");
        }

        [HarmonyPatch(typeof(PhysGrabObjectImpactDetector), "Break")]
        public class PhysGrabObjectImpactDetector_Break
        {
            static void Prefix(ref float valueLost, Vector3 _contactPoint, int breakLevel)
            {
                valueLost = ItemImpactBehavior.Value switch
                {
                    1 => float.MaxValue, // Break on impact
                    2 => 0f, // No damage
                    _ => valueLost // Default
                };
            }
        }
        
        [HarmonyPatch(typeof(ValuableObject), "DollarValueSetLogic")]
        public class ValuableObject_DollarValueSetLogic
        {
            static void Postfix(ref float ___dollarValueCurrent)
            {
                ___dollarValueCurrent *= DollarMultiplier.Value;
            }
        }
    }
}