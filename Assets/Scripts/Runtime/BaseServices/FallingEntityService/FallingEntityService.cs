using EEA.Game;
using System;
using System.Collections.Generic;

namespace EEA.GameService
{
    public class FallingEntityService : BaseService, IFallingEntityService
    {
        private HashSet<FallingEntity> _fallingEntities;

        public Action<FallingEntity> OnFallingEntityCollected { get; set; }

        public FallingEntityService()
        {
            _fallingEntities = new();
        }

        public void AddFallingEntity(FallingEntity fallingEntity)
        {
            _fallingEntities.Add(fallingEntity);
        }

        public void RemoveFallingEntity(FallingEntity fallingEntity)
        {
            _fallingEntities.Remove(fallingEntity);
        }

        public void ClearFallingEntity(FallingEntity fallingEntity)
        {
            if (!string.IsNullOrEmpty(fallingEntity.PlayerId))
            {
                OnFallingEntityCollected?.Invoke(fallingEntity);
            }

            BaseServices.PoolService.Despawn(fallingEntity);
        }
    }
}