using DG.Tweening;
using UnityEngine;

namespace EEA.Game
{
    public class BasePopup : MonoBehaviour
    {
        [SerializeField]
        private CanvasGroup canvasGroup;

        public void Show()
        {
            canvasGroup.DOFade(1, 0.5f)
                .From(0);
        }

        public void Hide()
        {
            canvasGroup.DOFade(0, 0.5f)
                .From(1);
        }
    }
}