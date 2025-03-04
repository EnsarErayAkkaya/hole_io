using System.Linq;
using UnityEngine;

namespace EEA.Game
{
    public class AIPlayer : PlayerBase
    {
        private FallingEntity _target;

        private void Update()
        {
            if (_target == null)
            {
                SelectTarget();
            }

            Move((_target.transform.position - transform.position).normalized);
        }

        private void SelectTarget()
        {
            var entities = BaseGameManager.FallingEntityService.FallingEntities;

            _target = entities.ElementAt(Random.Range(0, entities.Count));
        }
    }
}