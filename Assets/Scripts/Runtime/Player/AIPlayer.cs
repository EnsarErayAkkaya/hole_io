using EEA.BaseService;
using System.Collections.Generic;
using UnityEngine;

namespace EEA.Game
{
    public class AIPlayer : PlayerBase
    {
        /*private void Update()
        {
            
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
        }*/
    }
}