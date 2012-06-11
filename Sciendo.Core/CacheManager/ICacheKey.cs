using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sciendo.Core.CacheManager
{
    public interface ICacheKey
    {
        string[] KeyAttributes { get; }

        bool UseExactMatch { get; }
    }
}
