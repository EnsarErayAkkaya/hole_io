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
        public EditorReferences references;

        #region PROTECTED
        protected string _playerId;
        protected bool _isDestroyed;
        protected Rigidbody _rigidbody;
        protected MeshRenderer _meshRenderer;
        protected Collider _collider;
        #endregion PROTECTED

        #region PUBLIC

        public string PlayerId => _playerId;
        public bool IsDestroyed => _isDestroyed;

        public int RequiredSize
        {
            get => references.requiredSize;
            set => references.requiredSize = value;
        }

        public bool CanBeTransparent => references.canBeTransparent;
        #endregion PUBLIC

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _meshRenderer = GetComponent<MeshRenderer>();
            _collider = GetComponent<MeshCollider>();
        }

        private void OnEnable()
        {
            _playerId = null;
            _isDestroyed = false;
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.angularVelocity = Vector3.zero;

            BaseGameManager.FallingEntityService.AddFallingEntity(this);
        }

        private void OnDisable()
        {
            // if game ended return

            if (BaseGameManager.Instance != null)
                BaseGameManager.FallingEntityService.RemoveFallingEntity(this);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag(references.holeBottomTag) || _isDestroyed)
                return;

            this._playerId = other.GetComponentInParent<PlayerBase>().PlayerId;
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.CompareTag(references.holeDestroyTag) || _isDestroyed)
                return;

            _isDestroyed = true;

            BaseGameManager.FallingEntityService.ClearFallingEntity(this);
        }

        public virtual void SetFalling(int layer)
        {
            ChangeLayer(layer);
        }

        public virtual void SetNotFalling(int layer)
        {
            ChangeLayer(layer);
        }

        public virtual void ChangeLayer(int layer)
        {
            gameObject.layer = layer;
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

        public void SetMaterial(Material material)
        {
            _meshRenderer.sharedMaterial = material;
        }

        [Serializable]
        public class EditorReferences
        {
            [Range(1f, 20f)]
            public int requiredSize = 1;
            public bool canBeTransparent;
            [Tag] public string holeBottomTag;
            [Tag] public string holeDestroyTag;
        }
    }
}