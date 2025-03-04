using DG.Tweening;
using EEA.BaseService;
using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using TMPro;

namespace EEA.Game
{
    /// <summary>
    /// Abstract Base Class for every player in the game.
    /// </summary>

    [RequireComponent(typeof(NavMeshAgent))]
    public abstract class PlayerBase : MonoBehaviour
    {
        [SerializeField]
        public PlayerBaseEditorReferences references;

        #region PRIVATE
        private string _id;
        private string _playerName;
        private int _points;
        private int _xp;
        private int _level = 1;
        private float _speed = 1f;
        private float _size = 1f;
        private bool _isDead;
        private Color _color;
        private Transform _cachedTransform;
        private NavMeshAgent _agent;
        #endregion PRIVATE

        #region PUBLIC

        public string PlayerName
        {
            get => _playerName;
            set
            {
                _playerName = value;
                references.nameText.text = _playerName;
            }
        }

        public string PlayerId
        {
            get => _id;
            set => _id = value;
        }

        public int Points
        {
            get => _points;
            set => _points = value;
        }

        public int Xp => _xp;

        public int Level => _level;

        public float Speed => _speed;

        public float Size => _size;

        public bool IsDead => _isDead;
        public Color Color => _color;
        #endregion PUBLIC

        protected virtual void Awake()
        {
            _cachedTransform = transform;
            _agent = GetComponent<NavMeshAgent>();
            references.xpSlider.value = 0.0f;
            references.sliderText.text = $"0/{BaseGameManager.PlayerService.Settings.GetRequiredExpToLevelUp(_level)}";

            SetLevel(1);
        }

        /// <summary>
        /// Moves using AI Agent on Navmesh using offset. "offset" must be on XZ.
        /// </summary>
        /// <param name="offset"></param>
        public void Move(Vector3 offset)
        {
            _agent.Move(offset * _speed * Time.deltaTime);
        }

        public void AddXp(int xp, int requiredXp, int requiredXpNextLevel)
        {
            if (_isDead)
                return;

            _xp += xp;

            if (requiredXp == -1)
            {
                references.xpSlider.value = 1f;
                references.sliderText.gameObject.SetActive(false);

            }
            else if (_xp >= requiredXp && requiredXpNextLevel != -1)
            {
                SetLevel(_level + 1);
                _xp -= requiredXp;
                references.xpSlider.value = Mathf.Clamp01((float)_xp / (float)requiredXpNextLevel);
                references.sliderText.text = $"{_xp}/{requiredXpNextLevel}";

                BaseGameManager.PlayerService.PlayerLeveledUp(this);
            }
            else
            {
                references.xpSlider.value = Mathf.Clamp01((float)_xp / (float)requiredXp);
                references.sliderText.text = $"{_xp}/{requiredXp}";
            }
        }

        public void AddPoints(int pts) => _points += pts;

        public void SetLevel(int level)
        {
            _level = Mathf.Clamp(level, 0, 20);
            float levelProgressPercent = (float)(_level - 1) / 19f; // 0.0 - 1.0f

            SetSpeed(Mathf.Lerp(4f, 25f, levelProgressPercent));
            SetSize(Mathf.Lerp(2f, 40f, levelProgressPercent));

            foreach (var listener in references.holeResizeListeners)
            {
                listener.OnHoleSizeChanged(levelProgressPercent);
            }

            references.fallingEntityTrigger.SetMinimumSize(_level);

            references.levelText.text = $"LVL {_level}";
        }

        public void SetSpeed(float speed) => _speed = speed;

        public void SetSize(float size)
        {
            _size = size;
            _cachedTransform.localScale = new Vector3(size, 1, size);
        }

        public void SetColor(Color c)
        {
            _color = c;

            references.directionSprite.color = _color;
            references.skin.material.color = _color;
        }

        public void SetRotation(Quaternion rotation)
        {
            references.directionTransform.rotation = rotation;
        }

        public void SetPosition(Vector3 position) => _cachedTransform.position = position;
        public Vector3 GetPosition() => _cachedTransform.position;


        [Serializable]
        public class PlayerBaseEditorReferences
        {
            public HoleResizeListener[] holeResizeListeners;
            public HoleTrigger fallingEntityTrigger;

            public Transform directionTransform;
            public SpriteRenderer directionSprite;

            public MeshRenderer skin;

            [Header("Player Info")]
            public Slider xpSlider;
            public TextMeshProUGUI sliderText;
            public TextMeshProUGUI nameText;
            public TextMeshProUGUI levelText;
        }
    }
}
