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

        public CacheKey(bool fallOnMinimalKey, params string[] keyAttributes)
        {
            FallOnMinimalKey = fallOnMinimalKey;
            KeyAttributes = keyAttributes;
        }


        public bool FallOnMinimalKey
        {
            get;
            private set;
        }
    }
}
