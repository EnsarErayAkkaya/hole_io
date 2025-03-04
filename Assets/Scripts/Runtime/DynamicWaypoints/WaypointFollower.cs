using UnityEngine;

namespace EEA.Game
{
    public class WaypointFollower : MonoBehaviour
    {
        public EditorReferences references;

        #region PRIVATE
        private DynamicWaypointNode _fromNode;
        private DynamicWaypointNode _toNode;
        private bool _isFollowing = false;
        private float _sqrReachDist;
        #endregion PRIVATE

        #region PUBLIC
        public DynamicWaypointNode FromNode
        {
            get => _fromNode;
            set => _fromNode = value;
        }
        #endregion PUBLIC

        private void Start()
        {
            _sqrReachDist = references.reachedDistance * references.reachedDistance;
        }

        private void LateUpdate()
        {
            if (_isFollowing)
            {
                Vector3 direction = _toNode.transform.position - transform.position;

                transform.SetPositionAndRotation(
                    Vector3.MoveTowards(transform.position, _toNode.transform.position, references.speed * Time.deltaTime),
                    Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * references.rotationSpeed));

                // check if reached
                if (Vector3.SqrMagnitude(_toNode.transform.position - transform.position) <= _sqrReachDist)
                {
                    OnReached();
                }
            }
        }

        private void OnReached()
        {
            _fromNode = _toNode;
            _toNode = _fromNode.GetRandomNeighbour();
        }

        public void StartFollowing()
        {
            if (_fromNode != null && _toNode == null)
            {
                _toNode = _fromNode.GetRandomNeighbour();
            }

            if (WaypointManager.Instance.GetDistanceToProjection(_fromNode, _toNode, transform.position) < references.maxReattachDistance)
            {
                _isFollowing = true;

                transform.position = WaypointManager.Instance.GetProjectedPosition(_fromNode, _toNode, transform.position);
            }
        }

        public void StopFollowing()
        {
            _isFollowing = false;
        }

        [System.Serializable]
        public class EditorReferences
        {
            public float speed;
            public float rotationSpeed;
            [Tooltip("If the follower is further than this distance from the waypoint piece, it will not continue to follow.")]
            public float maxReattachDistance = 4;
            public float reachedDistance = 1;
        }
    }
}