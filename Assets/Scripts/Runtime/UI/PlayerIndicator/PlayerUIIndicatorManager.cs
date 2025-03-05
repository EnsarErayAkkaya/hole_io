using EEA.BaseService;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace EEA.Game
{
    public class PlayerUIIndicatorManager : MonoBehaviour
    {
        public EditorReferences references;

        private Dictionary<PlayerBase, PlayerUIIndicator> indicators = new();

        private void Awake()
        {
            BaseGameManager.Instance.OnServicesReady += OnServicesReady;
            BaseGameManager.Instance.OnGameCompleted += OnGameCompleted;
        }

        private void OnGameCompleted()
        {
            foreach (var item in indicators)
            {
                if (item.Value != null)
                {
                    BaseServices.PoolService.Despawn(item.Value);
                }
            }
        }

        private void OnServicesReady()
        {
            BaseGameManager.Instance.OnServicesReady -= OnServicesReady;

            BaseGameManager.PlayerService.OnPlayerCreated += OnPlayerCreated;
            BaseGameManager.PlayerService.OnPlayerDied += OnPlayerDied;
        }

        private void OnDestroy()
        {
            if (BaseGameManager.Instance != null)
            {
                BaseGameManager.PlayerService.OnPlayerCreated -= OnPlayerCreated;
                BaseGameManager.PlayerService.OnPlayerDied -= OnPlayerDied;
                BaseGameManager.Instance.OnGameCompleted -= OnGameCompleted;
            }
        }

        private void FixedUpdate()
        {
            foreach (var item in indicators)
            {
                item.Value.UpdateIndicator();
            }
        }

        private void OnPlayerCreated(PlayerBase playerBase)
        {
            if (playerBase is Player) return;

            var instance = BaseServices.PoolService.Spawn(references.indicatorPrefab);
            instance.transform.SetParent(references.parent, false);
            instance.Init(playerBase);

            indicators.Add(playerBase, instance);
        }

        private void OnPlayerDied(PlayerBase playerBase)
        {
            if (indicators.ContainsKey(playerBase))
            {
                BaseServices.PoolService.Despawn(indicators[playerBase]);
                indicators[playerBase].OnClear();
                indicators.Remove(playerBase);
            }
        }

        [Serializable]
        public class EditorReferences
        {
            public Transform parent;
            public PlayerUIIndicator indicatorPrefab;
        }
    }
}