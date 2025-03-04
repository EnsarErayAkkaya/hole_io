using EEA.Game;
using System.Collections.Generic;
using UnityEngine;
namespace EEA.Game
{
    [CreateAssetMenu(fileName = "TransparencyServiceSettings", menuName = "GameServices/Transparency Service Settings", order = 1)]
    public class TransparencyServiceSettings : ScriptableObject
    {
        public Material opaqueMat;
        public Material transparentMat;
        public LayerMask transparencyCheckLayermask;
    }
}