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

        

        public void ChangeLayer(int layer)
        {
            gameObject.layer = layer;
        }

        

        [Serializable]
        public class EditorReferences
        {
            [Range(1f, 20f)]
            public int sizeRequired = 1;
            [Range(1f, 20f)]
            public int points = 1;
            public bool canBeTransparent;
            public bool pooledObject;
        }
    }
}