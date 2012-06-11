using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sciendo.Core.CacheManager;

namespace Tests.Mocks
{
    public interface IMockClassUsingCache
    {
        [CacheKey(true,"attr1")]
        string CachedMethodForFullKeyOnly(string attr1);

        [CacheKey(true, "attr1")]
        string CachedMethodForFullKeyOnlyCachePresent(string attr1);

        [CacheKey(false,"attr1", "attr2")]
        string CacheMethodForFullAndMinimalKey(string attr1, string attr2);

        [CacheKey(false,"attr1", "attr2")]
        string CacheMethodForFullAndMinimalKeyCachePresent(string attr1, string attr2);

        [CacheKey(false,"attr1", "attr2")]
        string CacheMethodForFullAndMinimalKeyMinimalCachePresent(string attr1, string attr2);
    }
}
