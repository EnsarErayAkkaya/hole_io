using EEA.BaseService;
using UnityEngine;

namespace EEA.Game
{
    public class TimedGameManager : BaseGameManager
    {
        protected override void InitializeGame()
        {
            base.InitializeGame();

            GameStart();
        }

        public override void GameStart()
        {
            this._playerService.CreateUserPlayer(Vector3.zero);

            _cameraManager.SetCameraTarget(_playerService.Player);

            var config = BaseServices.LevelService.GetCurrentLevelConfig();

            for (int i = 0; i < _playerService.Settings.aiPlayerCount; i++)
            {
                _playerService.CreateAIPlayer(new Vector3(
                    Random.Range(-config.levelSize.x, config.levelSize.x),
                    0,
                    Random.Range(-config.levelSize.z, config.levelSize.z)));
            }
        }

        public override void GameEnd()
        {

        }
    }
}