using System;

namespace MiniProxy.Caching
{
    public interface ICache
    {
        T GetOrAdd<T>(string key, Func<T> factory);
    }
}