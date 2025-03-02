using EEA.Game;
using System.Collections.Generic;

namespace EEA.GameService
{
    public class FallingEntityService : BaseService, IFallingEntityService
    {
        private HashSet<FallingEntityController> _fallingEntities;

        public FallingEntityService()
        {
            _fallingEntities = new();
        }

        public void RegisterFallingEntity(FallingEntityController fallingEntity)
        {
            _fallingEntities.Add(fallingEntity);
        }

        public void UnregisterFallingEntity(FallingEntityController fallingEntity)
        {
            _fallingEntities.Remove(fallingEntity);

            GameServices.PoolService.Despawn(fallingEntity);
        }
    }
}