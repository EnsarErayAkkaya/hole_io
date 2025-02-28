using UnityEngine;
using EEA.Editor;

namespace EEA.Game
{
    public class Hole : MonoBehaviour
    {
        [SerializeField, Layer] private int entityLayer;
        [SerializeField, Layer] private int fallingEntityLayer;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.TryGetComponent(out Entity entity))
            {
                entity.ChangeLayer(fallingEntityLayer);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.TryGetComponent(out Entity entity))
            {
                entity.ChangeLayer(entityLayer);
            }
        }
    }
}