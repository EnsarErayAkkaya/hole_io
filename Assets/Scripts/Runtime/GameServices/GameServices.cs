using System;
using System.Collections.Generic;
using UnityEngine;

namespace EEA.GameService
{
    public class GameServices : MonoBehaviour
    {
        public Action OnServicesReady { get; set; }

        private List<ITickable> tickables = new();

        #region SERVICES
        private InputService inputService;

        public static IInputService InputService => instance.inputService;
        #endregion SERVICES

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