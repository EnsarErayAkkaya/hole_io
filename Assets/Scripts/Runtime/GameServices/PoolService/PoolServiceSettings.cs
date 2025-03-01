using System.Collections.Generic;
using UnityEngine;

namespace EEA.GameService
{
    [System.Serializable]
    public struct PoolInitializeData
    {
        [SerializeField] public GameObject poolItem;
        [SerializeField] public int preload;
        [SerializeField] public int capacity;
    }

    [CreateAssetMenu(fileName = "PoolServiceSettings", menuName = "GameServices/Pool Settings", order = 3)]
    public class PoolServiceSettings : ScriptableObject
    {
        public int defaultPoolCapacity = 100;
        public int defaultPoolPreload = 5;

        [SerializeField] private List<PoolInitializeData> poolInitializeData;

        public void InitializeDefinedPools()
        {
            foreach (var item in poolInitializeData)
            {
                InitializePoolItem(item);
            }
        }

        private void InitializePoolItem(PoolInitializeData data)
        {
            GameServices.PoolService.InitializePool(data.poolItem, data.preload, data.capacity);
        }
    }
}
