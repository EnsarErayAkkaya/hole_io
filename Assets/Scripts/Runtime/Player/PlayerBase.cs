using DG.Tweening;
using EEA.GameService;
using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

namespace EEA.Game
{
    /// <summary>
    /// Abstract Base Class for every player in the game.
    /// </summary>

    [RequireComponent(typeof(NavMeshAgent))]
    public abstract class PlayerBase : MonoBehaviour
    {
        #region SERIALIZED
        [SerializeField]
        public EditorReferences references;
        #endregion SERIALIZED

        #region PRIVATE
        private string _id;
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

        #region EVENTS
        public delegate void OnLevelChangedEventHandler(int level);
        public event OnLevelChangedEventHandler OnLevelChanged;
        #endregion EVENTS

        #region PUBLIC
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

        public int Level =>     _level;

        public float Speed => _speed;

        public float Size => _size;

        public bool IsDead => _isDead;
        #endregion PUBLIC

        private void Start()
        {
            InternalInit();
        }

        protected virtual void InternalInit()
        {
            _cachedTransform = transform;
            _agent = GetComponent<NavMeshAgent>();
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
                references.xpBarImage.fillAmount = 1f;
            }
            else if (_xp >= requiredXp && requiredXpNextLevel != -1)
            {
                SetLevel(_level + 1);
                _xp -= requiredXp;
                references.xpBarImage.fillAmount = Mathf.Clamp01((float)_xp / (float)requiredXpNextLevel);
            }
            else
                references.xpBarImage.fillAmount = Mathf.Clamp01((float)_xp / (float)requiredXp);
        }

        public void AddPoints(int pts) => _points += pts;

        public void SetLevel(int level)
        {
            _level = Mathf.Clamp(level, 0, 20);
            float levelProgressPercent = (float)(_level - 1) / 19f;

            SetSpeed(Mathf.Lerp(0.65f, 2.5f, levelProgressPercent));
            SetSize(Mathf.Lerp(2f, 40f, levelProgressPercent));

            references.fallingEntityTrigger.SetMinimumSize(_level);

            OnLevelChanged.Invoke(_level);
        }

        public void SetSpeed(float speed) => _speed = speed;

        public void SetSize(float size)
        {
            _size = size;
            _cachedTransform.localScale = new Vector3(size, 1f, size);
        }

        public void SetColor(Color c)
        {
            _color = c;

            references.directionSprite.color = _color;
            references.skin.color = _color;
        }

        public void SetRotation(Quaternion rotation)
        {
            references.directionTransform.rotation = rotation;
        }

        public void SetPosition(Vector3 position) => _cachedTransform.position = position;


        [Serializable]
        public class EditorReferences
        {
            public HoleTrigger fallingEntityTrigger;
            public Image xpBarImage;

            public Transform directionTransform;
            public SpriteRenderer directionSprite;

            public SpriteRenderer skin;
        }
    }
}
