using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sciendo.Core.CacheManager;

namespace Tests.Mocks
{
    internal class MockCacheManager:ICacheManager
    {
        public MockCacheManager()
        {
            MyOwnFakeCache = new Dictionary<string, object>();
        }

        internal Dictionary<string, object> MyOwnFakeCache;

        public void Add<T>(string key, T cacheItem, Type KnownType) where T : class
        {
            MyOwnFakeCache.Add(key, cacheItem);
        }

        public bool TryGet<T>(string cacheItemKey, out T cacheItem, Type knownType) where T : class
        {
            if (MyOwnFakeCache.ContainsKey(cacheItemKey))
            {
                cacheItem = MyOwnFakeCache[cacheItemKey] as T;
                return true;
            }
            cacheItem = null;
            return false;
        }

        public void Set<T>(string key, T cacheItem) where T : class
        {
            if (MyOwnFakeCache.ContainsKey(key))
                MyOwnFakeCache[key] = cacheItem;
            else
                MyOwnFakeCache.Add(key, cacheItem);
        }

        public void ForceRefresh()
        {
            throw new NotImplementedException();
        }
    }
}
