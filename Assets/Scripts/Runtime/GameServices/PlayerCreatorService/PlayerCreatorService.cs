using EEA.Game;
using System.Collections.Generic;
using UnityEngine;

namespace EEA.GameService
{
    public class PlayerCreatorService : MonoBehaviour
    {
        private List<PlayerData> players;

        private List<Color> allColors = new List<Color>()
        {
            Color.red,
            Color.blue,
            Color.green,
            Color.yellow,
            Color.cyan,
            Color.magenta,
            Color.black,
        };

        private List<Color> notUsedColors;

        public PlayerCreatorService()
        {
            players = new();
            notUsedColors = new List<Color>(allColors);
        }

        public void CreateUserPlayer()
        {
            
        }

        public void CreateAIPlayer()
        {
            
        }

        private void CreatePlayer()
        {
            int colorIndex = Random.Range(0, notUsedColors.Count);

            notUsedColors.RemoveAt(colorIndex);
        }
    }

    public class PlayerData
    {
        public string Guid { get; private set; }
        public Color Color { get; private set; }
        public int Level { get; private set; }
        public PlayerBase PlayerBase { get; }

        public PlayerData(PlayerBase player, int level, Color color)
        {
            PlayerBase = player;
            Level = level;
            Color = color;
        }
    }
}