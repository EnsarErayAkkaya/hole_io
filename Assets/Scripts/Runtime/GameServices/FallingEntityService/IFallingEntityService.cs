using EEA.Game;

namespace EEA.GameService
{
    public interface IFallingEntityService
    {
        public void RegisterFallingEntity(FallingEntityController fallingEntity);

        public void UnregisterFallingEntity(FallingEntityController fallingEntity);
    }
}