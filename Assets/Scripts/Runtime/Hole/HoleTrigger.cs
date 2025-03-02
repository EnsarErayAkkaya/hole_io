using UnityEngine;
using System;

namespace EEA.Game
{
    public class HoleTrigger : MonoBehaviour
    {
        [SerializeField]
        public EditorReferences references;
        private int _currentSize;

        public void SetMinimumSize(int size) => this._currentSize = size;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.TryGetComponent(out FallingEntityController entity))
            {
                entity.FallingEntity.ChangeLayer(references.fallingEntityLayer);
                entity.FallingEntity.WakeUpRigidbody();
            }
        }
        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.TryGetComponent(out FallingEntityController entity))
            {
                entity.FallingEntity.ChangeLayer(references.fallingEntityLayer);
                entity.FallingEntity.WakeUpRigidbody();
            }
            else if (other.gameObject.CompareTag(references.HoleTag))
            {
                
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.TryGetComponent(out FallingEntityController entity))
            {
                entity.FallingEntity.ChangeLayer(references.entityLayer);
                entity.FallingEntity.WakeUpRigidbody();
            }
        }

        [Serializable]
        public class EditorReferences
        {
            [Layer] public int entityLayer;
            [Layer] public int fallingEntityLayer;
            [Tag] public string HoleTag;
        }
    }
}