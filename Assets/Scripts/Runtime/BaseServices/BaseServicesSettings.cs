using UnityEngine;

namespace EEA.BaseService
{
    [CreateAssetMenu(fileName = "BaseServiceSettings", menuName = "BaseServices/Base Service Settings", order = 0)]
    public class BaseServicesSettings : ScriptableObject
    {
        public PoolServiceSettings PoolServiceSettings;
        public SceneServiceSettings SceneServiceSettings;

        public bool debugLog = true;
    }
}