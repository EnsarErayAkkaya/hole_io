using UnityEngine;

namespace EEA.Game
{
    /// <summary>
    /// Entity is objects Hole/Player can interact
    /// </summary>
    [RequireComponent(typeof(FallingEntity))]
    public class FallingEntityController : MonoBehaviour
    {
        #region PRIVATE
        private string _playerId;
        private bool _isDestroyed;
        private Rigidbody _rigidbody;
        private FallingEntity _fallingEntity;
        #endregion PRIVATE

        #region PUBLIC
        public FallingEntity FallingEntity => _fallingEntity;
        #endregion PUBLIC

        private void Start()
        {
            _fallingEntity = GetComponent<FallingEntity>();
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void OnEnable()
        {
            _playerId = "";
            _isDestroyed = false;
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.angularVelocity = Vector3.zero;
        }

        /// <summary>
        /// Wakes up connected rigidbody to include in physic calculations
        /// </summary>
        public void WakeUpRigidbody()
        {
            if (_rigidbody.IsSleeping())
                _rigidbody.WakeUp();
        }

        /// <summary>
        /// Sleeps rigidbody to not include in physic calculations 
        /// </summary>
        public void SleepRigidbody()
        {
            if (!_rigidbody.IsSleeping())
                _rigidbody.Sleep();
        }
    }
}