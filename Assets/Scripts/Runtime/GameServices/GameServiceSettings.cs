using UnityEngine;

namespace EEA.GameService
{
    [CreateAssetMenu(fileName = "GameServiceSettings", menuName = "GameServices/Game Service Settings", order = 0)]
    public class GameServiceSettings : ScriptableObject
    {
        public PlayerCreatorServiceSettings PlayerCreatorServiceSettings;
        public PoolServiceSettings PoolServiceSettings;

        public bool debugLog = true;
    }
}