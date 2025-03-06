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

        public void Clear()
        {
            BaseGameManager.PlayerService.OnPlayerCreated -= OnPlayerCreated;
        }

        public void Update()
        {
            if (_targetTransform == null)
                return;

            // calculate dir to target
            Vector3 directionToTarget = _targetTransform.position - _mainCamera.transform.position;
            Vector3 rayStart = _mainCamera.transform.position - directionToTarget * 2;
            Vector3 rayDirection = directionToTarget.normalized;
            float rayLength = directionToTarget.magnitude * 3;

            // raycast
            RaycastHit[] hits = Physics.RaycastAll(
                rayStart, rayDirection, rayLength,
                _settings.transparencyCheckLayermask,
                QueryTriggerInteraction.Ignore
            );

            _foundEntities.Clear();

            foreach (RaycastHit hit in hits)
            {
                if (hit.collider.gameObject.TryGetComponent(out FallingEntity entity) && entity.CanBeTransparent)
                {
                    entity.SetMaterial(_settings.transparentMat);
                    _foundEntities.Add(entity);
                }
            }

            // set entities no longer blocking the view as opaque
            _transparentEntities.RemoveWhere(entity =>
            {
                if (!_foundEntities.Contains(entity))
                {
                    entity?.SetMaterial(_settings.opaqueMat);
                    return true; // Remove from _transparentEntities
                }
                return false;
            });

            // merge new entities
            _transparentEntities.UnionWith(_foundEntities);
        }

    }
}