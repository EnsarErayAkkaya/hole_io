using System.Threading.Tasks;

namespace EEA.Game
{
    public class LosePopup : BasePopup
    {
        public async void OpenMenu()
        {
            Hide();

            await Task.Delay(350);

            UIManager.Instance.DestroyPopup(this);

            BaseGameManager.Instance.LoadMenu();
        }

        public async void Restart()
        {
            Hide();

            await Task.Delay(350);

            UIManager.Instance.DestroyPopup(this);

            BaseGameManager.Instance.RestartGame();
        }
    }
}