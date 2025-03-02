using System.Collections.Generic;
using UnityEngine;

namespace EEA.GameService
{
    public class PoolService : BaseService, IPoolService, ITickable
    {
        private PoolServiceSettings _settings;

        // All the currently active pools in the scene
        private List<Pool> _allPools = new List<Pool>();

        // The reference between a spawned GameObject and its pool
        private Dictionary<GameObject, Pool> _allLinks = new Dictionary<GameObject, Pool>();

        private Transform _poolsTransform = null;

        public PoolService(PoolServiceSettings poolServiceSettings)
        {
            this._settings = poolServiceSettings;

            _poolsTransform = new GameObject("Pools").transform;

            BaseServices.Instance.OnServicesReady += OnServicesReady;
        }

        private void OnServicesReady()
        {
            _settings.InitializeDefinedPools();
        }

        public void Tick()
        {
            foreach (var pool in _allPools)
            {
                // Go through all marked objects
                for (var i = pool.DelayedDestructions.Count - 1; i >= 0; i--)
                {
                    var markedObject = pool.DelayedDestructions[i];

                    // Is it still valid?
                    if (markedObject.Clone != null)
                    {
                        // Age it
                        markedObject.Life -= Time.deltaTime;

                        // Dead?
                        if (markedObject.Life <= 0.0f)
                        {
                            RemoveDelayedDestruction(pool, i);

                            // Despawn it
                            Despawn(markedObject.Clone);
                        }
                    }
                    else
                    {
                        RemoveDelayedDestruction(pool,i);
                    }
                }
            }
        }

        private void RemoveDelayedDestruction(Pool pool, int index)
        {
            var delayedDestruction = pool.DelayedDestructions[index];

            pool.DelayedDestructions.RemoveAt(index);

            ClassPool<DelayedDestruction>.Despawn(delayedDestruction);
        }

        public Pool InitializePool(GameObject prefab, int preload)
        {
            return InitializePool(prefab, preload, _settings.defaultPoolCapacity);
        }

        public Pool InitializePool(GameObject prefab, int preload, int capacity)
        {
            // Find the pool that handles this prefab
            var pool = _allPools.Find(p => p.Prefab == prefab);

            // Create a new pool for this prefab?
            if (pool == null)
            {
                pool = new Pool(prefab, new GameObject(prefab.name + " Pool").transform, capacity, preload);

                pool.PoolParent.transform.SetParent(this._poolsTransform);

                // Add new pool to AllPools list
                _allPools.Add(pool);
            }

            return pool;
        }

        // These methods allows you to spawn prefabs via Component with varying levels of transform data
        public T Spawn<T>(T prefab)
            where T : Component
        {
            return Spawn(prefab, Vector3.zero, Quaternion.identity, null);
        }

        public T Spawn<T>(T prefab, Vector3 position, Quaternion rotation)
            where T : Component
        {
            return Spawn(prefab, position, rotation, null);
        }

        public T Spawn<T>(T prefab, Vector3 position, Quaternion rotation, Transform parent)
            where T : Component
        {
            // Clone this prefabs's GameObject
            var gameObject = prefab != null ? prefab.gameObject : null;
            var clone = Spawn(gameObject, position, rotation, parent);

            // Return the same component from the clone
            return clone != null ? clone.GetComponent<T>() : null;
        }

        // These methods allows you to spawn prefabs via GameObject with varying levels of transform data
        public GameObject Spawn(GameObject prefab)
        {
            return Spawn(prefab, Vector3.zero, Quaternion.identity, null);
        }

        public GameObject Spawn(GameObject prefab, Vector3 position, Quaternion rotation)
        {
            return Spawn(prefab, position, rotation, null);
        }

        public GameObject Spawn(GameObject prefab, Vector3 position, Quaternion rotation, Transform parent)
        {
            if (prefab != null)
            {
                var pool = InitializePool(prefab, _settings.defaultPoolPreload);

                // Spawn a clone from this pool
                var clone = pool.FastSpawn(position, rotation, parent);

                // Was a clone created?
                // NOTE: This will be null if the pool's capacity has been reached
                if (clone != null)
                {
                    // Associate this clone with this pool
                    _allLinks.Add(clone, pool);

                    // Return the clone
                    return clone.gameObject;
                }
            }
            else
            {
                if (BaseServices.Instance.Settings.debugLog)
                    Debug.LogWarning("Attempting to spawn a null prefab");
            }

            return null;
        }

        // This allows you to despawn a clone via Component, with optional delay
        public void Despawn(Component clone, float delay = 0.0f)
        {
            if (clone != null)
                Despawn(clone.gameObject, delay);
        }

        // This allows you to despawn a clone via GameObject, with optional delay
        public void Despawn(GameObject clone, float delay = 0.0f)
        {
            if (clone != null)
            {
                Pool pool;

                // Try and find the pool associated with this clone
                if (_allLinks.TryGetValue(clone, out pool) == true)
                {
                    if (delay == 0)
                    {
                        // Remove the association
                        _allLinks.Remove(clone);
                    }

                    // Despawn it
                    pool.FastDespawn(clone, delay);
                }
                else
                {
                    if (_settings.warnWhenObjectPoolNotFoundOnDestroy)
                        Debug.LogWarning("Attempting to despawn " + clone.name + ", but failed to find pool for it! Make sure you created it using PoolService.Spawn!");

                    // Fall back to normal destroying
                    GameObject.Destroy(clone);
                }
            }
            else
            {
                //Debug.LogError("Attempting to despawn a null clone");
            }
        }
    }

    public class DelayedDestruction
    {
        public GameObject Clone;

        public float Life;
    }

    public class Pool
    {
        private GameObject _prefab;

        private int _capacity;

        // All the currently cached prefab instances
        private List<GameObject> _cache = new List<GameObject>();

        // All the delayed destruction objects
        private List<DelayedDestruction> _delayedDestructions = new List<DelayedDestruction>();

        // The total amount of created prefabs
        private int _total;

        private Transform _poolParent;

        public GameObject Prefab => _prefab;
        public int Capacity => _capacity;
        public List<GameObject> Cache => _cache;
        public List<DelayedDestruction> DelayedDestructions => _delayedDestructions;
        public Transform PoolParent => _poolParent;


        // Returns the total amount of spawned clones
        public int Total
        {
            get
            {
                return _total;
            }
        }

        // Returns the amount of cached clones
        public int Cached
        {
            get
            {
                return _cache.Count;
            }
        }

        public Pool(GameObject prefab, Transform poolParent)
        {
            this._prefab = prefab;
            this._poolParent = poolParent;
        }

        public Pool(GameObject prefab, Transform poolParent, int capacity)
        {
            this._prefab = prefab;
            this._poolParent = poolParent;
            this._capacity = capacity;
        }

        public Pool(GameObject prefab, Transform poolParent, int capacity, int preload)
        {
            this._prefab = prefab;
            this._poolParent = poolParent;
            this._capacity = capacity;

            if (_prefab != null)
            {
                for (var i = _total; i < preload; i++)
                {
                    FastPreload();
                }
            }
        }

        // This will return a clone from the cache, or create a new instance
        public GameObject FastSpawn(Vector3 position, Quaternion rotation, Transform parent = null)
        {
            if (_prefab != null)
            {
                // Attempt to spawn from the cache
                while (_cache.Count > 0)
                {
                    // Get last cache entry
                    var index = _cache.Count - 1;
                    var clone = _cache[index];

                    // Remove cache entry
                    _cache.RemoveAt(index);

                    if (clone != null)
                    {
                        // Update transform of clone
                        var cloneTransform = clone.transform;

                        cloneTransform.localPosition = position;

                        cloneTransform.localRotation = rotation;

                        cloneTransform.SetParent(parent, false);

                        // Activate clone
                        clone.SetActive(true);

                        // Messages?
                        SendNotification(clone, "OnSpawn");

                        return clone;
                    }
                    else
                    {
                        Debug.LogError("The " + _poolParent.name + " contained a null cache entry");
                    }
                }

                // Make a new clone?
                if (_capacity <= 0 || _total < _capacity)
                {
                    var clone = FastClone(position, rotation, parent);

                    // Messages?
                    SendNotification(clone, "OnSpawn");

                    return clone;
                }
            }
            else
            {
                Debug.LogError("Attempting to spawn null");
            }

            return null;
        }

        // Returns a clone of the prefab and increments the total
        // NOTE: Prefab is assumed to exist
        private GameObject FastClone(Vector3 position, Quaternion rotation, Transform parent)
        {
            var clone = GameObject.Instantiate(_prefab, position, rotation);

            _total += 1;

            clone.name = _prefab.name + " " + _total;

            clone.transform.SetParent(parent, false);

            return clone;
        }

        // This will despawn a clone and add it to the cache
        public void FastDespawn(GameObject clone, float delay = 0.0f)
        {
            if (clone != null)
            {
                // Delay the despawn?
                if (delay > 0.0f)
                {
                    // Make sure we only add it to the marked object list once
                    if (_delayedDestructions.Exists(m => m.Clone == clone) == false)
                    {
                        var delayedDestruction = ClassPool<DelayedDestruction>.Spawn() ?? new DelayedDestruction();

                        delayedDestruction.Clone = clone;
                        delayedDestruction.Life = delay;

                        _delayedDestructions.Add(delayedDestruction);
                    }
                }
                // Despawn now?
                else
                {
                    // Add it to the cache
                    _cache.Add(clone);

                    // Messages?
                    SendNotification(clone, "OnDespawn");

                    // Deactivate it
                    clone.SetActive(false);

                    // Move it under this GO
                    clone.transform.SetParent(_poolParent, false);
                }
            }
            else
            {
                //Debug.LogWarning("Attempting to despawn a null clone");
            }
        }

        // This allows you to make another clone and add it to the cache
        public void FastPreload()
        {
            if (_prefab != null)
            {
                // Create clone
                var clone = FastClone(Vector3.zero, Quaternion.identity, null);

                // Add it to the cache
                _cache.Add(clone);

                // Deactivate it
                clone.SetActive(false);

                // Move it under this GO
                clone.transform.SetParent(_poolParent, false);
            }
        }

        // Sends messages to clones
        // NOTE: clone is assumed to exist
        private void SendNotification(GameObject clone, string messageName)
        {
            clone.SendMessage(messageName, SendMessageOptions.DontRequireReceiver);
        }
    }
}
