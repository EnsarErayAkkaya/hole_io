using EEA.Game;
using System.Collections.Generic;
using UnityEngine;

namespace EEA.GameService
{
    [CreateAssetMenu(fileName = "PlayerServiceSettings", menuName = "GameServices/Player Service Settings", order = 1)]
    public class PlayerServiceSettings: ScriptableObject
    {
        public Player playerPrefab;
        public AIPlayer aiPlayerPrefab;

        [SerializeField] private List<int> _requiredExpsToLevelUp;
        [SerializeField] private List<int> _pointsForEntityLevel;


        // LEVEL START FROM 1 so I started lists above from 0 = 0
        //

        public int GetRequiredExpToLevelUp(int level)
        {
            if (level < _requiredExpsToLevelUp.Count)
                return _requiredExpsToLevelUp[level];
            else
                return -1;
        }

        public int GetPointsForEntityLevel(int level)
        {
            if (level < _pointsForEntityLevel.Count)
                return _pointsForEntityLevel[level];
            else
                return 0;
        }
    }
}