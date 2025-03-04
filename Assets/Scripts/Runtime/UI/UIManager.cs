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
        }

        public void ShowFloatingText(string str)
        {
            if (BaseGameManager.PlayerService.Player == null)
                return;

            TextMeshProUGUI text = BaseServices.PoolService.Spawn(references.floatingTextPrefab);

            text.transform.SetParent(references.floatingTextParent, false);

            text.transform.SetAsLastSibling();
            text.fontSize = 60;
            text.color = Color.white;
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
            DOTween.Kill(references.killText);

            references.killText.text = references.killTexts[count].ToUpper();

            references.killText.rectTransform.DOScale(Vector3.one, 0.3f)
                .From(3)
                .SetEase(Ease.OutBack)
                .OnComplete(() =>
                {
                    references.killText.rectTransform.DOShakePosition(0.2f)
                        .SetId(references.killText);
                })
                .SetId(references.killText);


            references.killText.DOFade(0.0f, 0.3f)
                .SetId(references.killText);
        }

        [Serializable]
        public class EditorReferences
        {
            public Transform floatingTextParent;
            public TextMeshProUGUI floatingTextPrefab;
            public TextMeshProUGUI killText;
            public string[] killTexts;
        }
    }
}