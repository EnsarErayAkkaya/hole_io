using System;
using System.Collections.Generic;
using UnityEngine;

namespace EEA.GameService
{
    public class GameServices : MonoBehaviour
    {
        [SerializeField] private GameServiceSettings settings;

        #region PRIVATE
        private List<ITickable> tickables = new();
        #endregion PRIVATE

        #region PUBLIC
        public GameServiceSettings Settings => settings;
        #endregion PUBLIC

        #region SERVICES
        private InputService inputService;
        private PlayerCreatorService playerCreatorService;
        private PoolService poolService;
        private FallingEntityService fallingEntityService;

        public static IInputService InputService => instance.inputService;
        public static IPlayerCreatorService PlayerCreatorService => instance.playerCreatorService;
        public static IPoolService PoolService => instance.poolService;
        public static IFallingEntityService FallingEntityService => instance.fallingEntityService;
        #endregion SERVICES

        #region EVENTS
        public delegate void OnServicesReadyHandler();
        public event OnServicesReadyHandler OnServicesReady;
        #endregion EVENTS

        #region SINGLETON
        private static GameServices instance;
        public static GameServices Instance => instance;
        #endregion SINGLETON

        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(instance);
            }

            instance = this;

            // Bind Services and interfaces
            inputService = BindServiceInterfaces<InputService>(new InputService());
            playerCreatorService = BindServiceInterfaces<PlayerCreatorService>(new PlayerCreatorService(settings.PlayerCreatorServiceSettings));
            poolService = BindServiceInterfaces<PoolService>(new PoolService(settings.PoolServiceSettings));
            fallingEntityService = BindServiceInterfaces<FallingEntityService>(new FallingEntityService());

            OnServicesReady?.Invoke();
        }

        private void Update()
        {
            foreach (var t in tickables)
            {
                t.Tick();
            }
        }

        private T BindServiceInterfaces<T>(BaseService baseService) where T : BaseService
        {
            if (baseService is ITickable)
            {
                tickables.Add((ITickable)baseService);
            }

            return (T)baseService;
        }
    }
}