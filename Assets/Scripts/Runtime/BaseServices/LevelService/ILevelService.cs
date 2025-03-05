using System.Threading.Tasks;

namespace EEA.BaseService
{
    public interface ILevelService
    {
        public int CurrentLevel { get; }
        public Task LoadNextLevel();
        public LevelServiceSettings.LevelConfig GetCurrentLevelConfig();
        public void LevelCompleted();
    }
}