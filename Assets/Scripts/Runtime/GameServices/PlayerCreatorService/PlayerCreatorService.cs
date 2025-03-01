using EEA.Game;
using System.Collections.Generic;
using UnityEngine;

namespace EEA.GameService
{
    public class PlayerCreatorService : BaseService, IPlayerCreatorService
    {
        #region PRIVATE
        private PlayerCreatorServiceSettings _settings;
        private Dictionary<string, PlayerBase> _playersDict;

        private List<Color> _notUsedColors;
        private List<Color> _allColors = new List<Color>()
        {
            Color.red,
            Color.blue,
            Color.green,
            Color.yellow,
            Color.cyan,
            Color.magenta,
            Color.black,
        };
        #endregion PRIVATE

        #region PUBLIC
        public Dictionary<string, PlayerBase> PlayersDict => _playersDict;
        public PlayerCreatorServiceSettings Settings => _settings;
        #endregion PUBLIC

        public PlayerCreatorService(PlayerCreatorServiceSettings settings)
        {
            this._settings = settings;
            _playersDict = new();
            _notUsedColors = new List<Color>(_allColors);
        }

        public PlayerBase CreateUserPlayer(Vector3 position)
        {
            var player = GameServices.PoolService.Spawn(_settings.playerPrefab);

            player.SetPosition(position);
            return CreatePlayer(player);
        }

        public PlayerBase CreateAIPlayer(Vector3 position)
        {
            var aiPlayer = GameServices.PoolService.Spawn(_settings.aiPlayerPrefab);

            return CreatePlayer(aiPlayer);
        }

        private PlayerBase CreatePlayer(PlayerBase playerBase)
        {
            string playerId = System.Guid.NewGuid().ToString();
            playerBase.PlayerId = playerId;

            int colorIndex = UnityEngine.Random.Range(0, _notUsedColors.Count);
            playerBase.SetColor(_notUsedColors[colorIndex]);

            _playersDict.Add(playerId, playerBase);


            // reset all colors after colors used
            _notUsedColors.RemoveAt(colorIndex);
            if (_notUsedColors.Count <= 0)
                _notUsedColors.AddRange(_allColors);

            return playerBase;
        }

    }
}