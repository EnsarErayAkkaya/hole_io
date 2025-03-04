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
        protected WaypointManager _waypointManager;

        public static IFallingEntityService FallingEntityService => Instance._fallingEntityService;
        public static IPlayerService PlayerService => Instance._playerService;
        public static IWaypointManager WaypointManager => Instance._waypointManager;
        #endregion SERVICES

        #region PUBLIC
        public GameState GameState => _gameState;
        #endregion PUBLIC

        #region EVENTS
        public delegate void OnServicesReadyHandler();
        public event OnServicesReadyHandler OnServicesReady;
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
            _waypointManager = new WaypointManager();

            InitializeGame();
        }

        protected virtual void InitializeGame()
        {
            _cameraManager = FindObjectOfType<CameraManager>();

            OnServicesReady?.Invoke();
        }

        private void Update()
        {
            _transparencyService.Update();
        }

        public abstract void GameStart();
        public abstract void GameEnd();

        public void RestartGame()
        {
            BaseServices.SceneService.LoadGameScene();
        }

        [Serializable]
        public class EditorReferences
        {
            public PlayerServiceSettings playerServiceSettings;
            public TransparencyServiceSettings transparencyServiceSettings;

        }
    }
}