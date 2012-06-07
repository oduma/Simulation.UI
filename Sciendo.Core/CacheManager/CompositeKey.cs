using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sciendo.Core.CacheManager
{
    public class CompositeKey
    {
        public string FullKey { get; set; }

        public string MinimalKey { get; set; }
    }
}
