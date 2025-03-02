using System.Threading.Tasks;
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

            _cameraManager.SetCameraTarget(_playerService.Player.transform);
        }
     
        public override void GameEnd()
        {
            
        }
    }
}