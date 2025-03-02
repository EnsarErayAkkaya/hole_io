using System;
using UnityEngine;

namespace EEA.Game
{
    public class CameraManager : MonoBehaviour
    {
        [SerializeField]
        public EditorReferences references;

        public void SetCameraTarget(Transform target)
        {
            references.cameraFollow.Target = target;
        }

        public void UpdateCameraCoom(float distance, float height)
        {
            this.references.cameraFollow.SetDistance(distance);
            this.references.cameraFollow.SetHeight(height);
        }

        [Serializable]
        public class EditorReferences
        {
            public CameraFollow cameraFollow;
        }
    }
}