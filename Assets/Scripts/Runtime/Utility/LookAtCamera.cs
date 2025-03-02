using UnityEngine;

namespace EEA.Game
{
    public class LookAtCamera : MonoBehaviour
    {
        private Transform _mainCamera;
        private Transform _cachedTransform;

        private void Awake()
        {
           _cachedTransform = transform;
        }

        private void OnEnable()
        {
            _mainCamera = Camera.main.transform;
        }

        private void LateUpdate()
        {
            _cachedTransform.rotation = Quaternion.LookRotation(_cachedTransform.position - _mainCamera.position);
        }
    }
}