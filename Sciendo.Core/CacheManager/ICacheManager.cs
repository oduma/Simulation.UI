using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sciendo.Core.CacheManager
{
    public interface ICacheManager
    {
        void Add<T>(string key, T cacheItem) where T: class;

        bool TryGet<T>(string cacheItemKey, out T cacheItem) where T: class;

        void Set<T>(string key, T cacheItem) where T: class;

        void ForceRefresh();

    }
}
