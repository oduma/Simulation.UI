using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.DynamicProxy;

namespace Sciendo.Core.CacheManager
{
    public abstract class CacheProviderBase:StandardInterceptor,ICacheProvider
    {
        private static object _lock = new object();

        protected abstract CompositeKey CalculateKey(IInvocation invocation, string[] keyAttributes = null);

        protected abstract ICacheManager Cache { get; }

        protected override void PerformProceed(IInvocation invocation)
        {
            
            CacheKey ck = (CacheKey)invocation.Method.GetCustomAttributes(typeof(CacheKey), false).FirstOrDefault();

            if (ck != null)
                GetFromCacheOrRun(invocation, ck);
            else
                base.PerformProceed(invocation);
        }

        private void GetFromCacheOrRun(IInvocation invocation, ICacheKey ck)
        {
            object result = null;
            lock (_lock)
            {
                CompositeKey key =  CalculateKey(invocation, (ck != null && ck.KeyAttributes != null && ck.KeyAttributes.Length > 0)?ck.KeyAttributes:null);
                
                if (Cache.TryGet(key.FullKey, out result))
                    invocation.ReturnValue = result;
                else
                {
                    base.PerformProceed(invocation);
                    if (invocation.ReturnValue != null)
                        Cache.Add(key.FullKey, invocation.ReturnValue);
                    else
                        if (Cache.TryGet(key.MinimalKey, out result))
                            invocation.ReturnValue = result;
                }
            }
        }
    }
}
