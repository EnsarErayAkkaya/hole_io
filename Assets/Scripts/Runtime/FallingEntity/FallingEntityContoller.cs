using EEA.GameService;
using System;
using UnityEngine;

namespace EEA.Game
{
    /// <summary>
    /// Entity is objects Hole/Player can interact
    /// </summary>
    [RequireComponent(typeof(FallingEntity))]
    public class FallingEntityController : MonoBehaviour
    {
        [SerializeField]
        public EditorReferences references;

        #region PRIVATE
        private string _playerId;
        private bool _isDestroyed;
        private FallingEntity _fallingEntity;
        #endregion PRIVATE

        #region PUBLIC
        public FallingEntity FallingEntity => _fallingEntity;
        #endregion PUBLIC

        private void Awake()
        {
            _fallingEntity = GetComponent<FallingEntity>();
        }

        private void OnEnable()
        {
            _playerId = "";
            _isDestroyed = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag(references.holeBottomTag))
                return;
            this._playerId = other.GetComponentInParent<PlayerBase>().PlayerId;
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.CompareTag(references.holeDestroyTag))
                return;

            GameServices.FallingEntityService.UnregisterFallingEntity(this);
        }

        [Serializable]
        public class EditorReferences
        {
            [Tag] public string holeBottomTag;
            [Tag] public string holeDestroyTag;
        }
    }
}