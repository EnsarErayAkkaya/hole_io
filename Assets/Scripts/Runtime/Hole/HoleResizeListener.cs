using System;
using UnityEngine;

namespace EEA.Game
{
    public class HoleResizeListener : MonoBehaviour
    {
        public EditorReferences references;

        public virtual void OnHoleSizeChanged(float percent)
        {
            references.transform.localScale = Vector3.Lerp(references.minSize, references.maxSize, percent);
        }

        [Serializable]
        public class EditorReferences
        {
            public Transform transform;
            public Vector3 minSize;
            public Vector3 maxSize;
        }
    }
}