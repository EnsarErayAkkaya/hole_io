using EEA.BaseService;
using TMPro;
using UnityEngine;

namespace EEA.Game
{
    public class MenuUIController : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI levelText;

        private void Start()
        {
            levelText.text = $"Level {BaseServices.LevelService.CurrentLevel + 1}";
        }

        public void LoadLevel()
        {
            BaseServices.LevelService.LoadNextLevel();
        }
    }
}