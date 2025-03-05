using DG.Tweening;
using EEA.BaseService;
using System;
using TMPro;
using UnityEngine;

namespace EEA.Game
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField]
        public EditorReferences references;

        private Camera mainCamera;

        private static UIManager instance;
        public static UIManager Instance => instance;

        private void Start()
        {
            instance = this;

            mainCamera = Camera.main;

            references.killCountText.text = "0";
        }

        public void UpdateTimer(int remainingSeconds)
        {
            int minutes = (remainingSeconds / 60);
            int seconds = (remainingSeconds % 60);
            references.timerText.text = $"{minutes.ToString("D2")}:{seconds.ToString("D2")}";
        }

        public void ShowXpCollectedText(string str)
        {
            if (BaseGameManager.PlayerService.Player == null)
                return;

            TextMeshProUGUI text = BaseServices.PoolService.Spawn(references.floatingTextPrefab);

            text.transform.SetParent(references.floatingTextParent, false);

            text.transform.SetAsLastSibling();
            text.text = str;
            text.transform.position = mainCamera.WorldToScreenPoint(BaseGameManager.PlayerService.Player.transform.position);

            DOTween.Kill(text);

            text.DOFade(0.0f, 0.5f)
                .SetDelay(0.5f)
                .OnKill(() =>
                {
                    if (text == null)
                        return;

                    BaseServices.PoolService.Despawn(text);
                })
                .SetId(text);

            text.rectTransform.DOAnchorPosY(70f, 1f)
                .SetEase(Ease.Linear)
                .SetId(text);
        }

        public void ShowKill(int count)
        {
            // update kill count
            references.killCountText.text = count.ToString();

            references.killFloatingText.gameObject.SetActive(true);
            DOTween.Kill(references.killFloatingText);

            references.killFloatingText.text = references.killTexts[count - 1].ToUpper();

            references.killFloatingText.DOFade(1f, 0.2f)
                .From(0)
                .SetId(references.killFloatingText);

            references.killFloatingText.rectTransform.DOScale(Vector3.one, 0.5f)
                .From(3)
                .SetEase(Ease.OutBack)
                .OnComplete(() =>
                {
                    references.killFloatingText.rectTransform.DOShakePosition(0.3f)
                        .SetId(references.killFloatingText);

                    references.killFloatingText.DOFade(0.0f, 0.4f)
                        .From(1)
                        .SetId(references.killFloatingText)
                        .OnComplete(() =>
                        {
                            references.killFloatingText.gameObject.SetActive(false);
                        });
                })
                .SetDelay(0.2f)
                .SetId(references.killFloatingText);
        }

        public void CreatePopup(BasePopup popup)
        {
            var instance = BaseServices.PoolService.Spawn(popup);

            if (instance != null)
            {
                instance.transform.SetParent(references.floatingTextParent, false);
                instance.transform.SetAsLastSibling();

                instance.Show();
            }
        }

        public void DestroyPopup(BasePopup popup)
        {
            BaseServices.PoolService.Despawn(popup);
        }

        [Serializable]
        public class EditorReferences
        {
            [Header("Floating Text")]
            public Transform floatingTextParent;
            public TextMeshProUGUI floatingTextPrefab;

            [Header("Kill Related")]
            public TextMeshProUGUI killFloatingText;
            public TextMeshProUGUI killCountText;
            public string[] killTexts;

            [Header("Timer")]
            public TextMeshProUGUI timerText;

            [Header("Popups")]
            public BasePopup winPopup;
            public BasePopup losePopup;
        }
    }
}