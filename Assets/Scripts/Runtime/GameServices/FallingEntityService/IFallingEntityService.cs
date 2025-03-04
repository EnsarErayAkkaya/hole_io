using System;

namespace EEA.Game
{
    public interface IFallingEntityService
    {
        public Action<FallingEntity> OnFallingEntityCollected { get; set; }

        public void AddFallingEntity(FallingEntity fallingEntity);

        public void RemoveFallingEntity(FallingEntity fallingEntity);

        public void ClearFallingEntity(FallingEntity fallingEntity);
    }
}