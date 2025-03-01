using EEA.Game;
using System.Collections.Generic;
using UnityEngine;

namespace EEA.GameService
{
    [CreateAssetMenu(fileName = "PlayerCreatorServiceSettings", menuName = "GameServices/Player Creator Service Settings", order = 1)]
    public class PlayerCreatorServiceSettings: ScriptableObject
    {
        public Player playerPrefab;
        public AIPlayer aiPlayerPrefab;

        [SerializeField] private List<float> _levelSizes;
        [SerializeField] private List<int> _requiredExpsToLevelUp;


        public int GetRequiredExpToLevelUp(int level)
        {
            return _requiredExpsToLevelUp[level - 1];
        }

        public float GetLevelSize(int level)
        {
            return _levelSizes[level - 1];
        }
    }
}