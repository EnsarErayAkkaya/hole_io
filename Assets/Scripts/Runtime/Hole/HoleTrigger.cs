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
            if (other.gameObject.TryGetComponent(out FallingEntity entity))
            {
                if (entity.RequiredSize > _currentSize)
                    return;

                entity.SetFalling(references.fallingEntityLayer);
                entity.WakeUpRigidbody();
            }
        }
        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.TryGetComponent(out FallingEntity entity))
            {
                if (entity.RequiredSize > _currentSize)
                    return;

                entity.SetFalling(references.fallingEntityLayer);
                entity.WakeUpRigidbody();
            }
            else if (other.gameObject.CompareTag(references.HoleTag))
            {
                // KILL OTHER HOLE
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.TryGetComponent(out FallingEntity entity))
            {
                if (entity.RequiredSize > _currentSize)
                    return;

                entity.SetNotFalling(references.entityLayer);
                entity.WakeUpRigidbody();
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