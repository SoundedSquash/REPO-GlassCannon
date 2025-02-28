using System.Collections.Generic;
using UnityEngine;

namespace GlassCannon.Patches
{
    public class ValuableObjectManager : MonoBehaviour
    {
        public static ValuableObjectManager Instance { get; private set; }
    
        private float DollarMultiplier = Settings.DollarMultiplier.Value;

        private List<ValuableObject> allValuableObjects = new List<ValuableObject>();

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
        }

        public void AddValuableObject(ValuableObject obj)
        {
            if (!allValuableObjects.Contains(obj))
            {
                allValuableObjects.Add(obj);
            }
        }

        public void RemoveValuableObject(ValuableObject obj)
        {
            allValuableObjects.Remove(obj);
        }

        private List<ValuableObject> GetAllValuableObjects()
        {
            return new List<ValuableObject>(allValuableObjects);
        }
    
        private void RemoveAllValuableObjects()
        {
            allValuableObjects.Clear();
            Settings.Logger.LogDebug("allValuableObjects cleared");
        }

        public void MultiplyValuableObjectsValue()
        {
            Settings.Logger.LogDebug("MultiplyValuableObjectsValue Started");
            var valuables = GetAllValuableObjects();
            Settings.Logger.LogDebug($"MultiplyValuableObjectsValue {valuables.Count} items");
            foreach (var valuable in valuables)
            {
                var value = valuable.dollarValueOriginal * DollarMultiplier;
                Settings.Logger.LogDebug($"Changed {valuable.name} value from {valuable.dollarValueOriginal} to {value}");
                
                valuable.dollarValueOriginal = value;
                valuable.dollarValueCurrent = value;
            }
        
            // Values have been processed. Clear references.
            RemoveAllValuableObjects();
            
            Settings.Logger.LogDebug("MultiplyValuableObjectsValue Ended");
        }
    }
}