using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tests.Mocks
{
    public class MockClassUsingCache:IMockClassUsingCache
    {
        public string CachedMethodForFullKeyOnly(string attr1)
        {
            return attr1 + " - Processed!!!";
        }

        public string CacheMethodForFullAndMinimalKey(string attr1, string attr2)
        {
            return attr1 + attr2 + " - Processed!!!";
        }


        public string CachedMethodForFullKeyOnlyCachePresent(string attr1)
        {
            throw new Exception("Cache Not Working!!!");
        }


        public string CacheMethodForFullAndMinimalKeyCachePresent(string attr1, string attr2)
        {
            throw new Exception("Cache Not Working!!!");
        }


        public string CacheMethodForFullAndMinimalKeyMinimalCachePresent(string attr1, string attr2)
        {
            return null;
        }
    }
}
