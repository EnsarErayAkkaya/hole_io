using UnityEngine;

namespace EEA.Game
{
    [RequireComponent(typeof(WaypointFollower))]
    public class MovableFallingEntity : FallingEntity
    {
        [SerializeField]
        private WaypointFollower _waypointFollower;

        public WaypointFollower WaypointFollower => _waypointFollower;

        public void Init()
        {
            _waypointFollower.StartFollowing();
        }

        public override void SetFalling(int layer)
        {
            SetKinematic(false);

            base.SetFalling(layer);

            _rigidbody.velocity = Vector3.zero;
            _rigidbody.angularVelocity = Vector3.zero;

            _waypointFollower.StopFollowing();
        }

        public override void SetNotFalling(int layer)
        {
            SetKinematic(true);

            base.SetNotFalling(layer);

            _waypointFollower.StartFollowing();
        }

        private void SetKinematic(bool isKinematic)
        {
            _rigidbody.isKinematic = isKinematic;

            _collider.isTrigger = isKinematic;
            
        }
    }
}