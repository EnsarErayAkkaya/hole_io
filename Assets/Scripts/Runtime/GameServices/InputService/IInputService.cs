using System;
using System.Collections.Generic;
using UnityEngine;

namespace EEA.GameService
{
    public enum InputType
    {
        Tap, Drag, Swipe, Pinch, Up, Hold
    }

    public interface IInputService
    {
        public Action<InputType, List<Vector3>> OnInputReceived{ get; set; }
    }
}