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

        private void Awake()
        {
            _mainCamera = Camera.main;
        }

        public void Init(PlayerBase target)
        {
            _player = target;

            references.levelText.text = $"LVL {_player.Level}";
            references.levelText.color = target.Color;

            references.nameText.text = target.PlayerName;
            references.nameText.color = target.Color;
            
            references.arrowImage.color = target.Color;
        }

        public void UpdatePosition()
        {
            if (_player == null || _player.IsDead)
                return;

            Vector3 playerPos = _player.GetPosition();

            // adjust z position accoring to player
            if (playerPos.z < _mainCamera.transform.position.z - 15.0)
                playerPos.z = _mainCamera.transform.position.z - 15f;

            Vector3 viewportPoint = _mainCamera.WorldToViewportPoint(playerPos);

            // Is player in wiewport
            if (viewportPoint.x >= 0.0 && viewportPoint.x <= 1.0 && viewportPoint.y >= 0.0 && viewportPoint.y <= 1.0)
            {
                gameObject.SetActive(false);
            }
            else
            {
                gameObject.SetActive(true);

                // rotate rect to point target
                references.rectTransform.transform.localRotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, Mathf.Atan2(viewportPoint.z, viewportPoint.x) + 180f));

                // clamp position
                viewportPoint.x = Mathf.Clamp(viewportPoint.x, 0.05f, 0.95f);
                viewportPoint.y = Mathf.Clamp(viewportPoint.y, 0.1f, 0.95f);

                // set min max anchors to clamp rect in position
                references.rectTransform.anchorMin = new Vector2(viewportPoint.x, viewportPoint.y);
                references.rectTransform.anchorMax = new Vector2(viewportPoint.x, viewportPoint.y);
            }
        }

        [Serializable]
        public class EditorReferences
        {
            public RectTransform rectTransform;
            public TextMeshProUGUI levelText;
            public Text nameText;
            public Image arrowImage;
        }
    }
}