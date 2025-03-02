using Eflatun.SceneReference;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace EEA.GameService
{
    public class SceneService : BaseService, ISceneService
    {
        private SceneServiceSettings settings;

        public SceneServiceSettings Settings => settings;

        public Action<SceneConfig> OnSceneTransitionStarted { get; set; }

        public Action<SceneConfig> OnSceneTransitionCompleted { get; set; }

        public SceneService(SceneServiceSettings _settings)
        {
            settings = _settings;

            BaseServices.Instance.OnServicesReady += Initialize;
        }

        public async void Initialize()
        {
            if (settings.loadTestScene)
            {
                await LoadScene(settings.testSceneConfig);
            }
            else if (settings.splashSceneConfig != null)
            {
                await LoadSplashScene();
            }
            else if (settings.menuSceneConfig != null)
            {
                await LoadMenuScene();
            }
            else if (settings.gameSceneConfig != null)
            {
                await LoadGameScene();
            }
        }

        public async Task LoadGameScene()
        {
            await LoadScene(settings.gameSceneConfig);
        }

        public async Task LoadMenuScene()
        {
            await LoadScene(settings.menuSceneConfig);
        }
        public async Task LoadSplashScene()
        {
            await LoadScene(settings.splashSceneConfig);
        }

        public async Task LoadBaseScene()
        {
            await LoadScene(settings.resolveBaseSceneConfig);
        }

        public async Task LoadScene(SceneConfig sceneConfig, float delay = 0)
        {
            if (sceneConfig == null) return;

            SceneReference[] sceneReferences = GetOpenScenes();

            OnSceneTransitionStarted?.Invoke(sceneConfig);

            if (delay != 0)
            {
                await Task.Delay(TimeSpan.FromSeconds(delay));
            }
       
            if (sceneConfig.removeAllOtherScenes)
            {
                List<Task> tasks = new List<Task>();
                for (int i = 0; i <  sceneReferences.Length; i++)
                {
                    if (sceneReferences[i].BuildIndex == settings.resolveBaseSceneConfig.sceneReference.BuildIndex) continue;

                    tasks.Add(RemoveScene(sceneReferences[i]));
                }

                await Task.WhenAll(tasks);
            }

            await LoadScene(sceneConfig.sceneReference, sceneConfig.loadMode);

            OnSceneTransitionCompleted?.Invoke(sceneConfig);
        }

        public async Task RemoveScene(SceneConfig sceneConfig, float delay = 0)
        {
            if (sceneConfig == null || !IsSceneActive(sceneConfig)) return;

            if (delay != 0)
            {
                await Task.Delay(TimeSpan.FromSeconds(delay));
            }

            await RemoveScene(sceneConfig.sceneReference);
        }

        private async Task LoadScene(SceneReference sceneReference, LoadSceneMode loadMode)
        {
            await AwaitCompletion(SceneManager.LoadSceneAsync(sceneReference.BuildIndex, loadMode));
        }
        private async Task RemoveScene(SceneReference sceneReference)
        {
            await AwaitCompletion(SceneManager.UnloadSceneAsync(sceneReference.BuildIndex));
        }

        private Task AwaitCompletion(AsyncOperation asyncOp)
        {
            var tcs = new TaskCompletionSource<bool>();
            asyncOp.completed += _ => tcs.SetResult(true);
            return tcs.Task;
        }

        private SceneReference[] GetOpenScenes()
        {
            int sceneCount = SceneManager.sceneCount;

            SceneReference[] sceneReferences = new SceneReference[sceneCount];

            for (int i = 0; i < sceneCount; i++)
            {
                Scene scene = SceneManager.GetSceneAt(i);

                sceneReferences[i] = settings.GetSceneReferenceByBuildIndex(scene.buildIndex);
            }

            return sceneReferences;
        }

        private bool IsSceneActive(SceneConfig sceneConfig)
        {
            SceneReference[] sceneReferences = GetOpenScenes();

            for (int i = 0; i < sceneReferences.Length; i++)
            {
                if (sceneReferences[i] != null && (sceneConfig.sceneReference.BuildIndex == sceneReferences[i].BuildIndex))
                {
                    return true;
                }
            }
            return false;
        }
    }
}