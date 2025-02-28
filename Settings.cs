using BepInEx.Configuration;
using BepInEx.Logging;

namespace GlassCannon
{
    public static class Settings
    {
        public static ConfigEntry<float> DollarMultiplier { get; private set; }
        public static float DollarMultiplierDefault = 2;

        public static ConfigEntry<int> ItemImpactBehavior { get; private set; }
        public static int ItemImpactBehaviorDefault = 1;
        
        public static ManualLogSource Logger { get; private set; }

        internal static void Initialize(ConfigFile config, ManualLogSource logger)
        {
            Logger = logger;
            
            DollarMultiplier = config.Bind(
                "General",
                "DollarMultiplier",
                2f,
                "The multiplier that is applied to item value. 2 doubles the value. 1 is the same. Any number between 0 and 1 will lose value.");
            
            ItemImpactBehavior = config.Bind(
                "General",
                "ItemImpactBehavior",
                1,
                @"0 - Default impacts.  1 - Break on impact.  2 - No impact damage.");
        }
    }
}