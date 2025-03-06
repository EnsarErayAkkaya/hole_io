using EEA.Game;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace EEA.BaseService
{
    public class PlayerService : IPlayerService
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

        private List<string> _botNames = new List<string>
        {
            "AlexRyder",
            "JakeStorm",
            "LiamShadow",
            "MasonVolt",
            "NoahSpecter",
            "EthanBlaze",
            "LucasFury",
            "LoganDrift",
            "OwenRogue",
            "AidenFrost",
            "RyanVortex",
            "CalebEcho",
            "NathanHavoc",
            "IsaacPhantom",
            "JulianFlare",
            "DylanStriker",
            "HunterNova",
            "ZachTitan",
            "ConnorHawk",
            "JaxonBlitz",
            "LeoStrike",
            "CameronDash",
            "TylerVenom",
            "BraydenGlitch",
            "DominicRaze",
            "EvanQuake",
            "AustinSurge",
            "GavinWarp",
            "AsherVandal",
            "ColeReaper"
        };

        private Player _player;
        #endregion PRIVATE

        #region PUBLIC
        public Dictionary<string, PlayerBase> PlayersDict => _playersDict;
        public PlayerServiceSettings Settings => _settings;
        public Player Player => _player;

        #endregion PUBLIC

        #region EVENTS
        public Action<PlayerBase> OnPlayerCreated { get; set; }
        public Action<PlayerBase> OnPlayerLevelUp { get; set; }
        public Action<PlayerBase> OnPlayerDied { get; set; }
        #endregion EVENTS

        public PlayerService(PlayerServiceSettings settings)
        {
            this._settings = settings;
            _playersDict = new();
            _notUsedColors = new List<Color>(_allColors);

            BaseGameManager.Instance.OnServicesReady += OnServicesReady;
        }

        private void OnServicesReady()
        {
            BaseGameManager.Instance.OnServicesReady -= OnServicesReady;

            BaseGameManager.FallingEntityService.OnFallingEntityCollected += OnFallingEntityCollected;
        }

        public void Clear()
        {
            BaseGameManager.FallingEntityService.OnFallingEntityCollected -= OnFallingEntityCollected;
        }

        private void OnFallingEntityCollected(FallingEntity entity)
        {
            if (_playersDict.TryGetValue(entity.PlayerId, out PlayerBase player))
            {
                int exp = _settings.GetPointsForEntityLevel(entity.RequiredLevel);
                player.AddXp(exp, _settings.GetRequiredExpToLevelUp(player.Level), _settings.GetRequiredExpToLevelUp(player.Level + 1));

                if (player is Player)
                {
                    UIManager.Instance.ShowXpCollectedText($"+{exp}");
                    Vibration.VibratePop();
                }
            }
        }

        public async void KillPlayer(PlayerBase killer, PlayerBase victim)
        {
            killer.AddXp(victim.Xp, _settings.GetRequiredExpToLevelUp(killer.Level), _settings.GetRequiredExpToLevelUp(killer.Level + 1));

            victim.Die();

            await Task.Delay(TimeSpan.FromSeconds(0.3f));

            BaseServices.PoolService.Despawn(victim.gameObject);

            if (killer is Player)
            {
                OnPlayerDied?.Invoke(victim);

                UIManager.Instance.ShowKill(++killer.KillCount);
#if UNITY_ANDROID
                Vibration.VibrateAndroid(700);
#endif
            }
            else if (victim is Player)
            {
                _player = null;
                BaseGameManager.Instance.CameraManager.SetCameraTarget(null);
            }

            _playersDict.Remove(victim.PlayerId);

            // if player dead
            if (_player == null)
            {
                BaseGameManager.Instance.GameEnd();
            }
            // all bots dead, player won
            else if (_playersDict.Count == 1)
            {
                BaseGameManager.Instance.GameEnd();
            }
        }

        public PlayerBase CreateUserPlayer(Vector3 position)
        {
            _player = BaseServices.PoolService.Spawn(_settings.playerPrefab);

            _player.SetPosition(position);

            _player.PlayerName = "Player";

            return CreatePlayer(_player);
        }

        public PlayerBase CreateAIPlayer(Vector3 position)
        {
            var aiPlayer = BaseServices.PoolService.Spawn(_settings.aiPlayerPrefab);

            aiPlayer.SetPosition(position);

            aiPlayer.PlayerName = _botNames[UnityEngine.Random.Range(0, _botNames.Count)];

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
            if (_notUsedColors.Count < 1)
                _notUsedColors.AddRange(_allColors);

            playerBase.Init();

            OnPlayerCreated?.Invoke(playerBase);

            return playerBase;
        }

        public void ClearPlayers()
        {
            foreach (var item in _playersDict)
            {
                BaseServices.PoolService.Despawn(item.Value.gameObject);
            }

            _playersDict.Clear();
            _player = null;
        }
        public void PlayerLeveledUp(PlayerBase playerBase)
        {
            if (playerBase is Player)
            {
#if UNITY_ANDROID
                Vibration.VibrateAndroid(1000);
#endif
            }

            OnPlayerLevelUp?.Invoke(playerBase);
        }
    }
}