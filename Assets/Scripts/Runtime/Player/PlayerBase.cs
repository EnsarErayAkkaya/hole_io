using EEA.BaseService;
using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using System.Threading.Tasks;

namespace EEA.Game
{
    /// <summary>
    /// Abstract Base Class for every player in the game.
    /// </summary>
    public abstract class PlayerBase : MonoBehaviour
    {
        [SerializeField]
        public PlayerBaseEditorReferences references;

        #region PRIVATE
        private string _id;
        private string _playerName;
        private int _xp;
        private int _level = 1;
        private float _speed = 1f;
        private float _size = 1f;
        private bool _isDead;
        private Color _color;
        private Transform _cachedTransform;
        private bool _canMove = true;
        private Vector3 _bounds;
        private int _killCount;
        #endregion PRIVATE

        #region PUBLIC
        public bool CanMove
        {
            get => this._canMove;
            set => this._canMove = value;
        }

        public int KillCount
        {
            get => this._killCount;
            set => this._killCount = value;
        }

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
        }

        public void Init()
        {
            _canMove = true;
            _isDead = false;
            _xp = 0;
            _level = 1;
            _size = 1;
            _killCount = 0;

            SetLevel(1);

            UpdateXpSlider(0f, $"0/{BaseGameManager.PlayerService.Settings.GetRequiredExpToLevelUp(_level)}");

            _bounds = BaseServices.LevelService.GetCurrentLevelConfig().levelSize;
        }

        /// <summary>
        /// Moves using AI Agent on Navmesh using offset. "offset" must be on XZ.
        /// </summary>
        /// <param name="offset"></param>
        public void Move(Vector3 offset)
        {
            if (!_canMove) return;

            offset.y = 0;

            Vector3 newPos = _cachedTransform.position + offset * Time.deltaTime * _speed;

            newPos += references.directionTransform.forward * (_size * 0.5f);
            
            if (IsPositionValid(newPos))
                _cachedTransform.position += offset * Time.deltaTime * _speed;
        }
        private bool IsPositionValid(Vector3 point)
        {
            return (point.x >= -_bounds.x && point.x <= _bounds.x) &&
                (point.z >= -_bounds.z && point.z <= _bounds.z);
        }

        public void AddXp(int xp, int requiredXp, int requiredXpNextLevel)
        {
            if (_isDead)
                return;

            _xp += xp;

            if (requiredXp == -1)
            {
                UpdateXpSlider(1f, "MAX");
            }
            else if (_xp >= requiredXp && requiredXpNextLevel != -1)
            {
                SetLevel(_level + 1);
                _xp -= requiredXp;

                UpdateXpSlider(Mathf.Clamp01((float)_xp / (float)requiredXpNextLevel), $"{_xp}/{requiredXpNextLevel}");

                BaseGameManager.PlayerService.PlayerLeveledUp(this);
            }
            else
            {
                UpdateXpSlider(Mathf.Clamp01((float)_xp / (float)requiredXp), $"{_xp}/{requiredXp}");
            }
        }

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

            references.fallingEntityTrigger.SetLevel(_level);

            references.levelText.text = $"LVL {_level}";
        }

        private void UpdateXpSlider(float value, string text)
        {
            if (references.xpSlider != null)
                references.xpSlider.value = value;

            if (references.sliderText != null)
            {
                if (text == null)
                {
                    references.sliderText.gameObject.SetActive(false);
                    references.sliderText.text = "";
                }
                else
                {
                    references.sliderText.gameObject.SetActive(true);
                    references.sliderText.text = text;
                }
            }
        }

        public void Die()
        {
            _isDead = true;

            transform.DOScale(Vector3.zero, 0.3f);
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
