using EEA.Game;
using System;
using UnityEngine;

namespace EEA.Game
{
    public interface IPlayerService
    {
        public Player Player { get; }

        public Action<PlayerBase> OnPlayerCreated { get; set; }
        public Action<PlayerBase> OnPlayerLevelUp { get; set; }


        public PlayerServiceSettings Settings { get; }
        public PlayerBase CreateUserPlayer(Vector3 position);

        public PlayerBase CreateAIPlayer(Vector3 position);

        public void PlayerLeveledUp(PlayerBase playerBase);

    }
}