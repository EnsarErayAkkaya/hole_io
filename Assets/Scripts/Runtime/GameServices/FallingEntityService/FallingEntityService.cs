using EEA.Game;
using System.Collections.Generic;

namespace EEA.GameService
{
    public class FallingEntityService : BaseService
    {
        private HashSet<FallingEntity> _fallingEntities;

        public FallingEntityService()
        {
            _fallingEntities = new();
        }

        public void RegisterFallingEntity(FallingEntity fallingEntity)
        {
            _fallingEntities.Add(fallingEntity);
        }

        public void UnregisterFallingEntity(FallingEntity fallingEntity)
        {
            _fallingEntities.Remove(fallingEntity);
        }
    }
}