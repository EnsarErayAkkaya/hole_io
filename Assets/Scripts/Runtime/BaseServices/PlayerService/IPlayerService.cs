using EEA.Game;
using System;
using UnityEngine;

namespace EEA.GameService
{
    public interface IPlayerService
    {
        public Player Player { get; }

        public Action<PlayerBase> OnPlayerCreated { get; set; }

        public PlayerServiceSettings Settings { get; }
        public PlayerBase CreateUserPlayer(Vector3 position);

        public PlayerBase CreateAIPlayer(Vector3 position);

    }
}