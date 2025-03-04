using System;
using System.Collections.Generic;

namespace EEA.Game
{
    public interface IFallingEntityService
    {
        public Action<FallingEntity> OnFallingEntityCollected { get; set; }
        public HashSet<FallingEntity> FallingEntities { get; }
        public void AddFallingEntity(FallingEntity fallingEntity);

        public void RemoveFallingEntity(FallingEntity fallingEntity);

        public void ClearFallingEntity(FallingEntity fallingEntity);
    }
}