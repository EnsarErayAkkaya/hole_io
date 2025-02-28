using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace EEA.Game
{
    /// <summary>
    /// Abstract Base Class for every player in the game.
    /// </summary>

    [RequireComponent(typeof(NavMeshAgent))]
    public abstract class PlayerBase : MonoBehaviour
    {
        private NavMeshAgent agent;

        #region PUBLIC
        public abstract int Level { get; }
        #endregion PUBLIC

        private void Start()
        {
            InternalInit();
        }

        protected virtual void InternalInit()
        {
            agent = GetComponent<NavMeshAgent>();
        }

        /// <summary>
        /// Moves using AI Agent on Navmesh using offset. "offset" must be on XZ.
        /// </summary>
        /// <param name="offset"></param>
        public void Move(Vector3 offset)
        {
            agent.Move(offset * agent.speed * Time.deltaTime);
        }
    }
}
