using System.Threading.Tasks;
using UnityEngine;

namespace EEA.BaseService
{
    public class LevelService : BaseService, ILevelService
    {
        private LevelServiceSettings _settings;
        private int _currentLevel;

        public int CurrentLevel => _currentLevel;

        public LevelService(LevelServiceSettings settings)
        {
            _settings = settings;
            _currentLevel = PlayerPrefs.GetInt("level", 0);    
        }

        public LevelServiceSettings.LevelConfig GetCurrentLevelConfig()
        {
            return _settings.GetLevelConfig(_currentLevel);
        }

        public async Task LoadNextLevel()
        {
            await BaseServices.SceneService.LoadScene(_settings.GetLevelConfig(_currentLevel).sceneConfig);
        }

        public void LevelCompleted()
        {
            PlayerPrefs.SetInt("level", ++_currentLevel);
        }
    }
}