using System.Collections;
using UnityEngine;

namespace EEA.GameService
{
    public class SceneTransitionController : MonoBehaviour
    {
        [SerializeField] private CanvasGroup transitionImage;

        private WaitForEndOfFrame waitForEndOfFrame = new();

        private Coroutine routine;

        private void Awake()
        {
            BaseServices.Instance.OnServicesReady += Initialize;
        }
        private void Initialize()
        {
            BaseServices.SceneService.OnSceneTransitionStarted += OnSceneTransitionStarted;
            BaseServices.SceneService.OnSceneTransitionCompleted += OnSceneTransitionCompleted;
        }

        private void OnSceneTransitionStarted(SceneConfig sceneConfig)
        {
            if (!sceneConfig.showSceneTransition) return;

            if (routine != null)
            {
                StopCoroutine(routine);
            }

            routine = StartCoroutine(TransitionEnumerator(.3f, 0, 1));
        }

        private void OnSceneTransitionCompleted(SceneConfig sceneConfig)
        {
            if (!sceneConfig.showSceneTransition) return;

            if (routine != null)
            {
                StopCoroutine(routine);
            }

            routine = StartCoroutine(TransitionEnumerator(.3f, 1, 0));
        }

        private IEnumerator TransitionEnumerator(float duration, float start, float alpha)
        {
            float t = 0;
            while (t < duration)
            {
                transitionImage.alpha = Mathf.Lerp(start, alpha, t / duration);

                t += Time.deltaTime;

                yield return waitForEndOfFrame;
            }

            transitionImage.alpha = alpha;
        }
    }
}