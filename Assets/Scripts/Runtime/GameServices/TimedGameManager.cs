using EEA.BaseService;
using System;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace EEA.Game
{
    public class TimedGameManager : BaseGameManager
    {
        public TimedModeEditorReferences timedModeReferences;

        protected override void InitializeGame()
        {
            base.InitializeGame();

            GameStart();
        }

        public override void GameStart()
        {
            var config = BaseServices.LevelService.GetCurrentLevelConfig();

            System.Func<Vector3> getRandomPos = () =>
            {
                return new Vector3(
                    UnityEngine.Random.Range(-config.levelSize.x, config.levelSize.x),
                    0,
                    UnityEngine.Random.Range(-config.levelSize.z, config.levelSize.z));
            };

            this._playerService.CreateUserPlayer(getRandomPos());

            _cameraManager.SetCameraTarget(_playerService.Player);

            for (int i = 0; i < _playerService.Settings.aiPlayerCount; i++)
            {
                _playerService.CreateAIPlayer(getRandomPos());
            }

            _gameState = GameState.GameStarted;

            StartCoroutine(TimerEnumerator());
        }

        private IEnumerator TimerEnumerator()
        {
            int duration = timedModeReferences.gameDurationSeconds;

            while (duration > 0 && _gameState == GameState.GameStarted)
            {
                yield return new WaitForSeconds(1);

                UIManager.Instance.UpdateTimer(--duration);
            }

            GameEnd();
        }

        public override void GameEnd()
        {
            if (_gameState == GameState.GameEnded)
                return;

            _gameState = GameState.GameEnded;

            foreach (var item in PlayerService.PlayersDict)
            {
                item.Value.CanMove = false;
            }

            if (_playerService.Player != null)
            {
                if (_playerService.PlayersDict.Any(s => s.Value.Level > _playerService.Player.Level))
                {
                    // there is a bigger hole
                    // lose
                    UIManager.Instance.CreatePopup(UIManager.Instance.references.losePopup);
                }
                else
                {
                    BaseServices.LevelService.LevelCompleted();
                    // win
                    UIManager.Instance.CreatePopup(UIManager.Instance.references.winPopup);
                }
            }
            else
            {
                // player died
                // lose
                UIManager.Instance.CreatePopup(UIManager.Instance.references.losePopup);
            }

            _playerService.ClearPlayers();

            OnGameCompleted?.Invoke();
        }

        [Serializable]
        public class TimedModeEditorReferences
        {
            public int gameDurationSeconds = 180;
        }
    }
}