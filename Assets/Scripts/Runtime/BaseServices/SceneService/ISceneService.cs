using Eflatun.SceneReference;
using System;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace EEA.BaseService
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

        public Task LoadScene(SceneReference sceneReference, LoadSceneMode loadMode);
        public Task RemoveScene(SceneReference sceneReference);

    }
}