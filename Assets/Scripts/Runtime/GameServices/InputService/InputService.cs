using Lean.Touch;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace EEA.GameService
{
    public class InputService : BaseService, IInputService
    {
        public Action<InputType, List<Vector3>> OnInputReceived { get; set; }

        public InputService()
        {
            LeanTouch.OnFingerUpdate += OnFingerUpdate;
        }

        private void OnFingerUpdate(LeanFinger finger)
        {
            if (finger.IsOverGui || finger.StartedOverGui) return;

            if (finger.Swipe)
            {
                OnInputReceived?.Invoke(InputType.Swipe, new List<Vector3>() { (Vector3)finger.StartScreenPosition, finger.ScreenPosition });

                return;
            }

            if (finger.Up)
            {
                OnInputReceived?.Invoke(InputType.Up, new List<Vector3>() { (Vector3)finger.ScreenPosition });

                return;
            }

            if (finger.Tap)
            {
                OnInputReceived?.Invoke(InputType.Tap, new List<Vector3>() { (Vector3)finger.ScreenPosition });

                return;
            }

            // if there is only one finger active
            if (LeanTouch.Fingers.Count < 2)
            {
                // if finger moved since start
                if (finger.SwipeScaledDelta.sqrMagnitude > 0.25f)
                {
                    OnInputReceived?.Invoke(InputType.Drag, new List<Vector3>() { (Vector3)finger.ScreenPosition, (Vector3)finger.StartScreenPosition });
                }
                else // finger not moved, just holding finger on screen 
                {
                    OnInputReceived?.Invoke(InputType.Hold, new List<Vector3> { (Vector3)finger.ScreenPosition });
                }
            }
        }
    }
}