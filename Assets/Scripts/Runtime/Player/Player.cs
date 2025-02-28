using EEA.GameService;
using System.Collections.Generic;
using UnityEngine;

namespace EEA.Game
{
    public class Player : PlayerBase
    {
        [SerializeField] private float joystickRadius;
        public override int Level => throw new System.NotImplementedException();

        protected override void InternalInit()
        {
            base.InternalInit();
            GameServices.InputService.OnInputReceived += OnInputReceived;
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
                offset = Vector3.ClampMagnitude(offset, joystickRadius);

                Vector3 percentedOffset = offset / joystickRadius;

                //Debug.Log($"Offset: {offset}, percentedOffset: {percentedOffset}");

                this.Move(percentedOffset);
            }
        }
    }
}
