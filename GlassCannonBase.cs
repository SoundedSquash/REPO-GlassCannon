using System.Collections;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using GlassCannon.Patches;
using HarmonyLib;
using UnityEngine;

namespace GlassCannon
{
    [BepInPlugin(PluginGuid, PluginName, PluginVersion)]
    public class GlassCannonBase : BaseUnityPlugin
    {
        private const string PluginGuid = "soundedsquash.glasscannon";
        private const string PluginName = "Glass Cannon";
        private const string PluginVersion = "1.0.0.0";
        
        private readonly Harmony _harmony = new Harmony(PluginGuid);

        private static readonly ManualLogSource ManualLogSource = BepInEx.Logging.Logger.CreateLogSource(PluginGuid);

        public void Awake()
        {
            // Initialize global objects
            Settings.Initialize(Config, ManualLogSource);
            _ = gameObject.AddComponent<ValuableObjectManager>();
            
            // Validate config
            if (!(Settings.DollarMultiplier.Value > 0f))
            {
                ManualLogSource.LogError(
                    $"Invalid Dollar Multiplier value {Settings.DollarMultiplier.Value}. Resetting to {Settings.DollarMultiplierDefault}");
                Settings.DollarMultiplier.Value = Settings.DollarMultiplierDefault;
            }
            
            if (Settings.ItemImpactBehavior.Value < 0 || Settings.ItemImpactBehavior.Value > 2)
            {
                ManualLogSource.LogError(
                    $"Invalid Item Impact Behavior value: {Settings.ItemImpactBehavior.Value}. Resetting to {Settings.ItemImpactBehaviorDefault}");
                Settings.ItemImpactBehavior.Value = Settings.ItemImpactBehaviorDefault;
            }

            _harmony.PatchAll();
            ManualLogSource.LogInfo($"{PluginName} loaded");
        }
    }
}