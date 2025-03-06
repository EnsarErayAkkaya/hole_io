using UnityEngine;

namespace EEA.Game
{
    [RequireComponent(typeof(WaypointFollower))]
    public class MovableFallingEntity : FallingEntity
    {
        [SerializeField]
        private WaypointFollower _waypointFollower;

        private float _settedFallingTime = 0;

        public WaypointFollower WaypointFollower => _waypointFollower;

        public void Init()
        {
            SetKinematic(true);
        }

        // IMPORTANT NOTE: WHEN RIGIDBODY isKinematic CHANGED, OnExitTrigger TRIGGERS INSTANTLY
        // I ADDED A TIMER TO FIX THIS ISSUE
        // SEE MORE: https://issuetracker.unity3d.com/issues/physics-rigidbody-ontriggerexit-slash-enter-methods-are-called-when-toggle-is-kinematic-on-slash-off

        public override void SetFalling(int layer)
        {
            // only set if rigidbody was kinematic and we are changing it
            if (_rigidbody.isKinematic)
                _settedFallingTime = Time.time;

            SetKinematic(false);

            base.SetFalling(layer);

            _waypointFollower.StopFollowing();
        }

        public override void SetNotFalling(int layer)
        {
            // Entity once fell down, cant set not falling
            if (transform.position.y < -1f) return;

            // if just set falling return
            if (_settedFallingTime + 0.1f > Time.time)
                return;

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