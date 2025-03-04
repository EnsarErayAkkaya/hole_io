using EEA.BaseService;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace EEA.Game
{
    public class Player : PlayerBase
    {
        [SerializeField]
        public PlayerEditorReferences playerReferences;

        private void OnEnable()
        {
            BaseServices.InputService.OnInputReceived += OnInputReceived;
        }

        private void OnDisable()
        {
            BaseServices.InputService.OnInputReceived -= OnInputReceived;
        }

        private void OnInputReceived(InputType type, List<Vector3> positions)
        {
            if (type == InputType.Drag)
            {
                // offset calculated and axis converted to XZ from XY
                Vector3 offset = positions[0] - positions[1];
                offset.z = offset.y;
                offset.y = 0;

                // clamp offst to joystick radius
                offset = Vector3.ClampMagnitude(offset, playerReferences.joystickRadius);

                Vector3 percentedOffset = offset / playerReferences.joystickRadius;

                //Debug.Log($"Offset: {offset}, percentedOffset: {percentedOffset}");

                SetRotation(Quaternion.Euler(0f, Mathf.Atan2(percentedOffset.x, percentedOffset.z) * Mathf.Rad2Deg, 0f));

                Move(percentedOffset);
            }
        }

        [Serializable]
        public class PlayerEditorReferences
        {
            public float joystickRadius;
            public Canvas worldCanvas;
        }
    }
}
