using UnityEngine;

namespace Resolve.BaseServices
{
    [CreateAssetMenu(fileName = "ResolveSettings", menuName = "Base Scriptable Objects/Resolve/ResolveSettings", order = 0)]
    public class ResolveSettings : ScriptableObject
    {

        public bool debugLog = true;
    }
}