using EEA.BaseService;
using System;
using UnityEngine;

namespace EEA.Game
{
    public enum GameState
    {
        GameLoading, GameStarted, GameEnded
    }

    public abstract class BaseGameManager : MonoBehaviour
    {
        [SerializeField]
        private EditorReferences references;

        #region PRIVATE
        protected GameState _gameState = GameState.GameLoading;
        protected CameraManager _cameraManager;
        #endregion PRIVATE

        #region SERVICES
        protected PlayerService _playerService;
        protected FallingEntityService _fallingEntityService = new();
        protected TransparencyService _transparencyService;
        protected WaypointService _waypointService;

        public static IFallingEntityService FallingEntityService => Instance._fallingEntityService;
        public static IPlayerService PlayerService => Instance._playerService;
        public static IWaypointService WaypointService => Instance._waypointService;
        #endregion SERVICES

        #region PUBLIC
        public GameState GameState => _gameState;
        public CameraManager CameraManager => _cameraManager;
        #endregion PUBLIC

        #region EVENTS
        public delegate void OnServicesReadyHandler();
        public event OnServicesReadyHandler OnServicesReady;

        public delegate void OnGameCompletedHandler();
        public OnGameCompletedHandler OnGameCompleted;
        #endregion EVENTS

        #region SINGLETON
        private static BaseGameManager _instance;
        public static BaseGameManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<BaseGameManager>();
                }
                return _instance;
            }
        }
        #endregion SINGLETON

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
            }

            _instance = this;

            _playerService = new PlayerService(references.playerServiceSettings);
            _transparencyService = new TransparencyService(references.transparencyServiceSettings);
            _waypointService = new WaypointService();

            InitializeGame();
        }


        private void OnDestroy()
        {
            _playerService.Clear();
            _transparencyService.Clear();
        }

        protected virtual void InitializeGame()
        {
            _cameraManager = FindObjectOfType<CameraManager>();

            OnServicesReady?.Invoke();
        }

        private void FixedUpdate()
        {
            _transparencyService.Update();
        }

        public abstract void GameStart();
        public abstract void GameEnd();

        public async void RestartGame()
        {
            var config = BaseServices.LevelService.GetCurrentLevelConfig();

            await BaseServices.SceneService.RemoveScene(config.sceneConfig);

            await BaseServices.SceneService.LoadScene(config.sceneConfig);
        }

        public void LoadMenu()
        {
            BaseServices.SceneService.LoadMenuScene();
        }

        [Serializable]
        public class EditorReferences
        {
            public PlayerServiceSettings playerServiceSettings;
            public TransparencyServiceSettings transparencyServiceSettings;

        }
    }
}