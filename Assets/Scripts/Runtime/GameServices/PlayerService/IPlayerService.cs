using EEA.Game;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace EEA.Game
{
    public interface IPlayerService
    {
        public Player Player { get; }

        public Action<PlayerBase> OnPlayerCreated { get; set; }
        public Action<PlayerBase> OnPlayerLevelUp { get; set; }
        public Action<PlayerBase> OnPlayerDied { get; set; }

        public PlayerServiceSettings Settings { get; }
        public Dictionary<string, PlayerBase> PlayersDict { get; }

        public void KillPlayer(PlayerBase killer, PlayerBase victim);

        public PlayerBase CreateUserPlayer(Vector3 position);

        public PlayerBase CreateAIPlayer(Vector3 position);

        public void PlayerLeveledUp(PlayerBase playerBase);

        public void ClearPlayers();
    }
}