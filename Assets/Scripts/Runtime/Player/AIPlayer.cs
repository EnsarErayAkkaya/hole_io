using System;
using System.Linq;
using UnityEngine;

namespace EEA.Game
{
    public class AIPlayer : PlayerBase
    {
        public EditorReferences aiReferences;

        private FallingEntity _target;
        private float _sqrSlowDownDistance;
        private float _sqrSelectTargetRadius;
        private float _reachedTime = 0;

        private void Start()
        {
            _sqrSlowDownDistance = aiReferences.slowDownDistance * aiReferences.slowDownDistance;
            _sqrSelectTargetRadius = aiReferences.selectTargetRadius * aiReferences.selectTargetRadius;
        }

        private void Update()
        {
            if (_target == null || _target.IsDestroyed)
            {
                SelectTarget();
            }

            Vector3 dir = (_target.transform.position - transform.position);

            if (dir.magnitude > 0.5f && _target.transform.position.y > -1)
            {
                SetRotation(Quaternion.Euler(0f, Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg, 0f));

                Move(dir.normalized * (dir.sqrMagnitude <= _sqrSlowDownDistance ? 0.5f : 1));
            }
            else
            {
                // reached target but waiting to collect entity for a while
                // if wait for more than reachedTargetMaxWaitDuration select a new target

                if (_reachedTime + aiReferences.reachedTargetMaxWaitDuration <= Time.time)
                {
                    _reachedTime = 0; // setting back to zero for being safer
                    SelectTarget();
                }
                else if (Time.time - _reachedTime > aiReferences.reachedTargetMaxWaitDuration)
                {
                    _reachedTime = Time.time;
                }
            }
        }

        private void SelectTarget()
        {
            if (BaseGameManager.Instance == null) return;

            do
            {
                var entities = BaseGameManager.FallingEntityService.FallingEntities;

                _target = entities.ElementAt(UnityEngine.Random.Range(0, entities.Count));

            } while ((_target.RequiredLevel > this.Level) || 
                ((_target.transform.position - transform.position).sqrMagnitude > _sqrSelectTargetRadius));
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(transform.position, aiReferences.selectTargetRadius);
        }

        [Serializable]
        public class EditorReferences
        {
            public float selectTargetRadius;
            public float slowDownDistance;
            public float reachedTargetMaxWaitDuration;
        }
    }
}