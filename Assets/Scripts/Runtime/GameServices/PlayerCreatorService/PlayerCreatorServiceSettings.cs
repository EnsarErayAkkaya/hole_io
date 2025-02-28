using EEA.Game;
using UnityEngine;

namespace EEA.GameService
{
    [CreateAssetMenu(fileName = "PlayerCreatorServiceSettings", menuName = "GameServices/Player Creator Service Settings")]
    public class PlayerCreatorServiceSettings: ScriptableObject
    {
        public Player playerPrefab;
    }
}