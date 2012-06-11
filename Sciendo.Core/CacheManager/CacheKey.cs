using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sciendo.Core.CacheManager
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class CacheKey : Attribute,ICacheKey
    {
        public string[] KeyAttributes { get; private set; }

        public CacheKey(bool useExactMatch, params string[] keyAttributes)
        {
            UseExactMatch = useExactMatch;
            KeyAttributes = keyAttributes;
        }


        public bool UseExactMatch
        {
            get;
            private set;
        }
    }
}
