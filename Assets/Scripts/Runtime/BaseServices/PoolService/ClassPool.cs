// This class allows you to pool normal C# classes, for example:
// var foo = ClassPool<Foo>.Spawn() ?? new Foo();
// ClassPool<Foo>.Despawn(foo);
using System.Collections.Generic;

namespace EEA.BaseService
{
    public static class ClassPool<T>
        where T : class
    {
        private static List<T> _cache = new List<T>();

        public static T Spawn()
        {
            return Spawn(null, null);
        }

        public static T Spawn(System.Action<T> onSpawn)
        {
            return Spawn(null, onSpawn);
        }

        public static T Spawn(System.Predicate<T> match)
        {
            return Spawn(match, null);
        }

        // This will either return a pooled class instance, or null
        // You can also specify a match for the exact class instance you're looking for
        // You can also specify an action to run on the class instance (e.g. if you need to reset it)
        // NOTE: Because it can return null, you should use it like this: ClassPool<Whatever>.Spawn(...) ?? new Whatever(...)
        public static T Spawn(System.Predicate<T> match, System.Action<T> onSpawn)
        {
            // Get the matched index, or the last index
            var index = match != null ? _cache.FindIndex(match) : _cache.Count - 1;

            // Was one found?
            if (index >= 0)
            {
                // Get instance and remove it from cache
                var instance = _cache[index];

                _cache.RemoveAt(index);

                // Run action?
                if (onSpawn != null)
                {
                    onSpawn(instance);
                }

                return instance;
            }

            // Return null?
            return null;
        }

        public static void Despawn(T instance)
        {
            Despawn(instance, null);
        }

        // This allows you to desapwn a class instance
        // You can also specify an action to run on the class instance (e.g. if you need to reset it)
        public static void Despawn(T instance, System.Action<T> onDespawn)
        {
            // Does it exist?
            if (instance != null)
            {
                // Run action on it?
                if (onDespawn != null)
                {
                    onDespawn(instance);
                }

                // Add to cache
                _cache.Add(instance);
            }
        }

        public static void Clear()
        {
            _cache.Clear();
        }
    }

    public static class ClassPool
    {
        private static List<object> _cache = new List<object>();

        public static T Spawn<T>() where T : class
        {
            for (int i = 0; i < _cache.Count; i++)
            {
                object instance = _cache[i];
                if (instance is T)
                {
                    _cache.RemoveAt(i);
                    return (T)instance;
                }
            }

            // Return null?
            return null;
        }

        public static void Despawn(object instance)
        {
            // Does it exist?
            if (instance != null)
            {
                // Add to cache
                _cache.Add(instance);
            }
        }

        public static void Clear()
        {
            _cache.Clear();
        }
    }
}