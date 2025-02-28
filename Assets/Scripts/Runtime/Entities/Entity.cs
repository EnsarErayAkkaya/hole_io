using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EEA.Game
{
    /// <summary>
    /// Entity is objects Hole/Player can interact
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    public class Entity : MonoBehaviour
    {
        [SerializeField] private int level;

        private new Rigidbody rigidbody;
        private void Start()
        {
            rigidbody = GetComponent<Rigidbody>();
        }

        public void ChangeLayer(int layer)
        {
            gameObject.layer = layer;
            WakeUpRigidbody();
        }

        /// <summary>
        /// Wakes up connected rigidbody to include physic calculations
        /// </summary>
        public void WakeUpRigidbody()
        {
            if (rigidbody.IsSleeping())
                rigidbody.WakeUp();
        }

        /// <summary>
        /// Sleeps rigidbody to not include physic calculations 
        /// </summary>
        public void SleepRigidbody()
        {
            if (!rigidbody.IsSleeping())
                rigidbody.Sleep();
        }
    }
}