using System;
using UnityEngine;

namespace EEA.Game
{
    /// <summary>
    /// Entity is objects Hole/Player can interact
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    public class FallingEntity : MonoBehaviour
    {
        [SerializeField]
        public EditorReferences references;

        #region PRIVATE
        private string _holeId;
        private bool _isDestroyed;
        private Rigidbody _rigidbody;

        #endregion PRIVATE

        #region PUBLIC
        public int SizeRequired
        {
            get => references.sizeRequired;
            set => references.sizeRequired = value;
        }

        public int Points
        {
            get => references.points;
            set => references.points = value;
        }

        public bool CanBeTransparent => references.canBeTransparent;
        #endregion PUBLIC

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void OnEnable()
        {
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.angularVelocity = Vector3.zero;
        }

        public void ChangeLayer(int layer)
        {
            gameObject.layer = layer;
            references.fallingEntityTrigger.layer = layer;
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

        [Serializable]
        public class EditorReferences
        {
            public GameObject fallingEntityTrigger;
            [Range(1f, 20f)]
            public int sizeRequired = 1;
            [Range(1f, 20f)]
            public int points = 1;
            public bool canBeTransparent;
        }
    }
}