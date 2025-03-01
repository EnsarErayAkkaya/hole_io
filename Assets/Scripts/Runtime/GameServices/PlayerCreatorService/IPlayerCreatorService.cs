using EEA.Game;
using UnityEngine;

namespace EEA.GameService
{
    public interface IPlayerCreatorService
    {
        public PlayerCreatorServiceSettings Settings { get; }
        public PlayerBase CreateUserPlayer(Vector3 position);

        public PlayerBase CreateAIPlayer(Vector3 position);

    }
}