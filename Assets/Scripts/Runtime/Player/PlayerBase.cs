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
        #region PRIVATE
        private NavMeshAgent agent;
        private string guid;
        private int level;
        #endregion PRIVATE
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

        public virtual void Init(string guid, int level)
        {
            this.guid = guid;
            this.level = level;
        }

        /// <summary>
        /// Moves using AI Agent on Navmesh using offset. "offset" must be on XZ.
        /// </summary>
        /// <param name="offset"></param>
        public void Move(Vector3 offset)
        {
            agent.Move(offset * agent.speed * Time.deltaTime);
        }

        protected void Scale(bool isInstant = false)
        {
            if (isInstant)
            {
                transform.localScale = Vector3.one * level;
            }
            else
            {

            }
        }
    }
}
