using System;

namespace MiniProxy.Caching
{
    public class NullCache : ICache
    {
        public T GetOrAdd<T>(string key, Func<T> factory)
        {
            return factory();
        }
    }
}