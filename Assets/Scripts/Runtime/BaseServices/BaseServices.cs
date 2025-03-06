using System.Collections.Generic;
using UnityEngine;

namespace EEA.BaseService
{
    public class BaseServices : MonoBehaviour
    {
        [SerializeField] private BaseServicesSettings settings;

        #region PRIVATE
        private List<ITickable> _tickables = new();
        #endregion PRIVATE

        #region PUBLIC
        public BaseServicesSettings Settings => settings;
        #endregion PUBLIC

        #region SERVICES
        private InputService inputService;
        private PoolService poolService;
        private SceneService sceneService;
        private LevelService levelService;

        public static IInputService InputService => instance.inputService;
        public static IPoolService PoolService => instance.poolService;
        public static ISceneService SceneService => instance.sceneService;
        public static ILevelService LevelService => instance.levelService;
        #endregion SERVICES

        #region EVENTS
        public delegate void OnServicesReadyHandler();
        public event OnServicesReadyHandler OnServicesReady;
        #endregion EVENTS

        #region SINGLETON
        private static BaseServices instance;
        public static BaseServices Instance => instance;
        #endregion SINGLETON

        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
            }

            instance = this;

            // Bind Services and interfaces
            levelService = BindServiceInterfaces<LevelService>(new LevelService(settings.LevelServiceSettings));
            inputService = BindServiceInterfaces<InputService>(new InputService());
            poolService = BindServiceInterfaces<PoolService>(new PoolService(settings.PoolServiceSettings));
            sceneService = BindServiceInterfaces<SceneService>(new SceneService(settings.SceneServiceSettings));

            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            OnServicesReady?.Invoke();

            Vibration.Init();
        }

        private void Update()
        {
            foreach (var t in _tickables)
            {
                t.Tick();
            }
        }

        private T BindServiceInterfaces<T>(BaseService baseService) where T : BaseService
        {
            if (baseService is ITickable)
            {
                _tickables.Add((ITickable)baseService);
            }

            return (T)baseService;
        }
    }
}