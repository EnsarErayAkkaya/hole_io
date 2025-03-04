using UnityEngine;

namespace EEA.BaseService
{
    public interface IPoolable
    {
        void OnSpawn();
        void OnDespawn();
        GameObject gameObject { get; }
        Transform transform{ get; }
    }
}