using UnityEngine;
using System;

namespace EEA.Game
{
    public class HoleTrigger : MonoBehaviour
    {
        [SerializeField]
        public EditorReferences references;
        private int _currentLevel;

        public void SetLevel(int size) => this._currentLevel = size;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.TryGetComponent(out FallingEntity entity))
            {
                if (entity.RequiredLevel > _currentLevel)
                    return;

                entity.SetFalling(references.fallingEntityLayer);
                entity.WakeUpRigidbody();
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.TryGetComponent(out FallingEntity entity))
            {
                if (entity.RequiredLevel > _currentLevel)
                    return;

                entity.SetFalling(references.fallingEntityLayer);
                entity.WakeUpRigidbody();
            }
            else if (other.gameObject.CompareTag(references.HoleTag))
            {
                // KILL OTHER HOLE
                var otherPlayer = other.gameObject.GetComponentInParent<PlayerBase>();

                if (otherPlayer != null && !otherPlayer.IsDead && otherPlayer.Level < _currentLevel)
                {
                    var player = gameObject.GetComponentInParent<PlayerBase>();

                    BaseGameManager.PlayerService.KillPlayer(player, otherPlayer);
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.TryGetComponent(out FallingEntity entity))
            {
                if (entity.RequiredLevel > _currentLevel)
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