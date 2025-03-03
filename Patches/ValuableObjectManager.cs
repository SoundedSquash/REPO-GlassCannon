using System.Collections.Generic;
using System.Linq;
using Photon.Pun;

namespace GlassCannon.Patches
{
    public static class ValuableObjectManager
    {
        private static float DollarMultiplier = Settings.DollarMultiplier.Value;

        private static List<ValuableObject> allValuableObjects = new List<ValuableObject>();

        public static void AddValuableObject(ValuableObject obj)
        {
            if (!allValuableObjects.Contains(obj))
            {
                allValuableObjects.Add(obj);
            }
        }

        private static List<ValuableObject> GetAllValuableObjects()
        {
            return new List<ValuableObject>(allValuableObjects.Where(valuable => valuable != null));
        }
    
        private static void RemoveAllValuableObjects()
        {
            allValuableObjects.Clear();
            Settings.Logger.LogDebug("allValuableObjects cleared");
        }

        public static void MultiplyValuableObjectsValue()
        {
            Settings.Logger.LogDebug("MultiplyValuableObjectsValue Started");

            if (GameManager.instance.gameMode == 0 || PhotonNetwork.IsMasterClient)
            {
                var valuables = GetAllValuableObjects();
                Settings.Logger.LogDebug($"MultiplyValuableObjectsValue {valuables.Count} items");
                foreach (var valuable in valuables)
                {
                    var value = valuable.dollarValueOriginal * DollarMultiplier;
                    Settings.Logger.LogDebug(
                        $"Changed {valuable.name} value from {valuable.dollarValueOriginal} to {value}");

                    valuable.dollarValueOriginal = value;
                    valuable.dollarValueCurrent = value;
                }
            }
            else
            {
                Settings.Logger.LogDebug("MultiplyValuableObjectsValue You are not the host. Skipping multiplication.");
            }

            // Values have been processed. Clear references.
            RemoveAllValuableObjects();
            
            Settings.Logger.LogDebug("MultiplyValuableObjectsValue Ended");
        }
    }
}