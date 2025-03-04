using System;
using UnityEngine;

namespace EEA.Game
{
    public class CameraManager : MonoBehaviour
    {
        [SerializeField]
        public EditorReferences references;

        private void Start()
        {
            BaseGameManager.PlayerService.OnPlayerLevelUp += OnPlayerLevelUp;
        }

        private void OnPlayerLevelUp(PlayerBase playerBase)
        {
            if (playerBase.transform == references.cameraFollow.Target)
            {
                UpdateCamera(playerBase);
            }
        }

        public void SetCameraTarget(PlayerBase playerBase)
        {
            if (playerBase is Player)
            {
                var player = playerBase as Player;
                player.playerReferences.worldCanvas.worldCamera = references.uiCamera;
            }
            
            references.cameraFollow.Target = playerBase.transform;

            UpdateCamera(playerBase);
        }

        private void UpdateCamera(PlayerBase playerBase)
        {
            float levelProgress = (float)(playerBase.Level - 1) / 19f;

            references.cameraFollow.SetDistance(Mathf.Lerp(5f, 80f, levelProgress));
            references.cameraFollow.SetHeight(Mathf.Lerp(7f, 100f, levelProgress));
        }

        [Serializable]
        public class EditorReferences
        {
            public CameraFollow cameraFollow;
            public Camera uiCamera;
        }
    }
}