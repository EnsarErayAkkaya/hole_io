using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace EEA.Game
{
    public class PlayerUIIndicator : MonoBehaviour
    {
        [SerializeField]
        public EditorReferences references;

        private Camera _mainCamera;
        private PlayerBase _player;

        private void OnEnable()
        {
            _mainCamera = Camera.main;
        }

        public void Init(PlayerBase target)
        {
            _player = target;

            references.levelText.text = $"LVL {_player.Level}";
            references.levelText.color = target.Color;

            references.playerNameText.text = target.PlayerName;
            references.playerNameText.color = target.Color;
            
            references.arrowImage.color = target.Color;

            BaseGameManager.PlayerService.OnPlayerLevelUp += OnPlayerLevelUp;
        }

        private void OnPlayerLevelUp(PlayerBase playerBase)
        {
            if (playerBase == _player)
                references.levelText.text = $"LVL {playerBase.Level}";
        }

        public void OnClear()
        {
            BaseGameManager.PlayerService.OnPlayerLevelUp -= OnPlayerLevelUp;
        }

        public void UpdateIndicator()
        {
            if (_player == null || _player.IsDead)
                return;

            Vector3 playerPos = _player.GetPosition();

            // Adjust z position according to player
            if (playerPos.z < _mainCamera.transform.position.z - 15.0f)
                playerPos.z = _mainCamera.transform.position.z - 15f;

            Vector3 viewportPoint = _mainCamera.WorldToViewportPoint(playerPos);

            // Check if player is in viewport
            if (viewportPoint.x >= 0.0f && viewportPoint.x <= 1.0f && viewportPoint.y >= 0.0f && viewportPoint.y <= 1.0f && viewportPoint.z > 0)
            {
                gameObject.SetActive(false);
            }
            else
            {
                gameObject.SetActive(true);

                // Flip when behind the camera
                if (viewportPoint.z < 0)
                {
                    viewportPoint.x = 1 - viewportPoint.x;
                    viewportPoint.y = 1 - viewportPoint.y;
                }

                // Rotate UI to point at the target
                float angle = Mathf.Atan2(viewportPoint.y - 0.5f, viewportPoint.x - 0.5f) * Mathf.Rad2Deg;
                references.rectTransform.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, angle + 90);

                // Clamp position
                float clampedX = Mathf.Clamp(viewportPoint.x, 0.1f, 0.9f);
                float clampedY = Mathf.Clamp(viewportPoint.y, 0.15f, 0.85f);

                references.rectTransform.anchorMin = new Vector2(clampedX, clampedY);
                references.rectTransform.anchorMax = new Vector2(clampedX, clampedY);

                // Ensure proper pivot
                references.rectTransform.pivot = new Vector2(0.5f, 0.5f);
            }
        }


        [Serializable]
        public class EditorReferences
        {
            public RectTransform rectTransform;
            public TextMeshProUGUI levelText;
            public TextMeshProUGUI playerNameText;
            public Image arrowImage;
        }
    }
}