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

            float y1 = _target.eulerAngles.y;
            float b =  _target.position.y + references.height * references.zoom;
            float y2 = transform.eulerAngles.y;
            float y3 = transform.position.y;
            float y4 = Mathf.LerpAngle(y2, y1, references.rotationDamping * Time.deltaTime);
            float y5 = Mathf.Lerp(y3, b, references.heightDamping * Time.deltaTime);

            Quaternion quaternion = Quaternion.Euler(0.0f, y4, 0.0f);

            _cachedTransform.position = _target.position;
            _cachedTransform.position -= quaternion * Vector3.forward * references.distance * references.zoom;
            _cachedTransform.position = new Vector3(transform.position.x, y5, transform.position.z);
            _cachedTransform.LookAt(_target);

            if (Mathf.Abs(_targetDistance - references.distance) > Mathf.Epsilon)
            {
                references.distance = Mathf.MoveTowards(references.distance, _targetDistance, Time.deltaTime * 30f);
            }
            if (Mathf.Abs(_targetHeight - references.height) <= Mathf.Epsilon)
            {
                return;
            }

            references.height = Mathf.MoveTowards(references.height, _targetHeight, Time.deltaTime * 30f);
        }

        public void SetZoom(float z) => references.zoom = z;

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