using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EEA.Game
{
    /// <summary>
    /// Entity is objects Hole/Player can interact
    /// </summary>
    public class Entity : MonoBehaviour
    {
        [SerializeField] private int level;

        public void ChangeLayer(int layer)
        {
            gameObject.layer = layer;
        }
    }
}