using System;
using System.Collections.Generic;
using UnityEngine;

namespace EEA.Game
{
    public class TransparencyService
    {
        private TransparencyServiceSettings _settings;
        private Camera _mainCamera;
        private Transform _targetTransform;
        private HashSet<FallingEntity> _transparentEntities = new();
        private List<FallingEntity> _foundEntities = new();

        public TransparencyService(TransparencyServiceSettings settings)
        {
            _settings = settings;
            _mainCamera = Camera.main;

            BaseGameManager.PlayerService.OnPlayerCreated += OnPlayerCreated;
        }

        private void OnPlayerCreated(PlayerBase playerBase)
        {
            if (playerBase is Player)
                _targetTransform = playerBase.transform;
        }

        public void Update()
        {
            if (_targetTransform == null)
                return;

            Vector3 rayDirection = _targetTransform.position - _mainCamera.transform.position;

            float maxDistance = rayDirection.magnitude;

            RaycastHit[] raycastHitArray = Physics.RaycastAll(_mainCamera.transform.position - rayDirection * 2, rayDirection.normalized, maxDistance * 3, _settings.transparencyCheckLayermask, QueryTriggerInteraction.Ignore);

            _foundEntities.Clear();

            if (raycastHitArray.Length > 0)
            {
                foreach (RaycastHit raycastHit in raycastHitArray)
                {
                    if (raycastHit.collider.gameObject.TryGetComponent<FallingEntity>(out var fallingEntity))
                    {
                        if (fallingEntity.CanBeTransparent)
                        {
                            fallingEntity.SetMaterial(_settings.transparentMat);
                            _foundEntities.Add(fallingEntity);
                        }
                    }
                }
            }

            _transparentEntities.RemoveWhere((FallingEntity alreadyTransparentEntity) =>
            {
                if (!_foundEntities.Contains(alreadyTransparentEntity))
                {
                    alreadyTransparentEntity.SetMaterial(_settings.opaqueMat);
                    return true;
                }

                return false;
            });

            foreach (var item in _foundEntities)
            {
                _transparentEntities.Add(item);
            }
        }
    }
}