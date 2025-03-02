using DG.Tweening.Core.Easing;
using EEA.Game;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace EEA.GameService
{
    public class PlayerService : BaseService, IPlayerService
    {
        #region PRIVATE
        private PlayerServiceSettings _settings;
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

        private Player _player;
        #endregion PRIVATE

        #region PUBLIC
        public Dictionary<string, PlayerBase> PlayersDict => _playersDict;
        public PlayerServiceSettings Settings => _settings;
        public Player Player => _player;

        #endregion PUBLIC
        public Action<PlayerBase> OnPlayerCreated { get; set; }

        public PlayerService(PlayerServiceSettings settings)
        {
            this._settings = settings;
            _playersDict = new();
            _notUsedColors = new List<Color>(_allColors);

            BaseGameManager.Instance.OnServicesReady += OnServicesReady;
        }

        private void OnServicesReady()
        {
            BaseGameManager.FallingEntityService.OnFallingEntityCollected += OnFallingEntityCollected;
        }

        private void OnFallingEntityCollected(FallingEntity entity)
        {
            if (_playersDict.TryGetValue(entity.PlayerId, out PlayerBase player))
            {
                int exp = _settings.GetPointsForEntityLevel(entity.RequiredSize);
                player.AddPoints(exp);
                player.AddXp(exp, _settings.GetRequiredExpToLevelUp(player.Level), _settings.GetRequiredExpToLevelUp(player.Level + 1));
            }
        }
        public PlayerBase CreateUserPlayer(Vector3 position)
        {
            _player = BaseServices.PoolService.Spawn(_settings.playerPrefab);

            _player.SetPosition(position);
            return CreatePlayer(_player);
        }

        public PlayerBase CreateAIPlayer(Vector3 position)
        {
            var aiPlayer = BaseServices.PoolService.Spawn(_settings.aiPlayerPrefab);

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

            OnPlayerCreated?.Invoke(playerBase);

            return playerBase;
        }

    }
}