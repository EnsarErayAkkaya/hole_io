using System;
using System.Threading.Tasks;

namespace EEA.GameService
{
    public interface ISceneService
    {
        public Action<SceneConfig> OnSceneTransitionStarted { get; set; }
        public Action<SceneConfig> OnSceneTransitionCompleted { get; set; }

        public SceneServiceSettings Settings { get; }

        public Task LoadBaseScene();
        public Task LoadSplashScene();
        public Task LoadMenuScene();
        public Task LoadGameScene();
        
        public Task LoadScene(SceneConfig sceneConfig, float delay = 0);
        public Task RemoveScene(SceneConfig sceneConfig, float delay = 0);
    }
}