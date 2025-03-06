using System;
using System.Threading.Tasks;
using UnityEngine;

namespace EEA.Game
{
    public class CameraFollow : MonoBehaviour
    {
        [SerializeField]
        public EditorReferences references;

        private Transform _target;
        private float _targetDistance;
        private float _targetHeight;
        private Transform _cachedTransform;

        private float _targetRotationY;
        private float _currentRotationY;
        private float _currentHeight;
        private float _smoothedRotationY;
        private float _smoothedHeight;
        public Transform Target
        {
            get => _target;
            set => _target = value;
        }

        private void Start()
        {
            _cachedTransform = transform;
            _targetDistance = references.distance;
            _targetHeight = references.height;
        }

        public void LateUpdate()
        {
            if (_target == null)
                return;

            // Extract target properties
            _targetRotationY = _target.eulerAngles.y + 45;
            float targetHeight = _target.position.y + references.height * references.zoom;

            // Extract current properties
            _currentRotationY = transform.eulerAngles.y;
            _currentHeight = transform.position.y;

            // Smoothly interpolate rotation and height
            _smoothedRotationY = Mathf.LerpAngle(_currentRotationY, _targetRotationY, references.rotationDamping * Time.deltaTime);
            _smoothedHeight = Mathf.Lerp(_currentHeight, targetHeight, references.heightDamping * Time.deltaTime);

            // Compute new camera position
            Quaternion rotation = Quaternion.Euler(0f, _smoothedRotationY, 0f);
            _cachedTransform.position = _target.position - rotation * Vector3.forward * references.distance * references.zoom;
            _cachedTransform.position = new Vector3(_cachedTransform.position.x, _smoothedHeight, _cachedTransform.position.z);

            // Look at the target
            _cachedTransform.LookAt(_target);

            // Adjust distance if needed
            if (!Mathf.Approximately(_targetDistance, references.distance))
            {
                references.distance = Mathf.MoveTowards(references.distance, _targetDistance, Time.deltaTime * 30f);
            }

            // Adjust height if needed
            if (!Mathf.Approximately(_targetHeight, references.height))
            {
                references.height = Mathf.MoveTowards(references.height, _targetHeight, Time.deltaTime * 30f);
            }
        }


        public void SetHeight(float h) => _targetHeight = h;

        public void SetDistance(float d) => _targetDistance = d;

        [Serializable]
        public class EditorReferences
        {
            public float distance = 10f;
            public float height = 5f;
            public float rotationDamping;
            public float heightDamping;
            public float zoom = 1f;
        }
    }
}