using EEA.Game;
using System;
using UnityEngine;

namespace EEA.GameService
{
    public class HoleCore : MonoBehaviour
    {
        [SerializeField]
        private EditorReferences references;

        private void OnTriggerStay(Collider other)
        {
            if (!other.gameObject.CompareTag(references.HoleTag))
                return;
        }

        [Serializable]
        public class EditorReferences
        {
            [Tag] public string HoleTag;
        }
    }
}