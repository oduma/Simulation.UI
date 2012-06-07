using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.DynamicProxy;

namespace Sciendo.Core.CacheManager
{
    public class CacheProviderForMethods:CacheProviderBase
    {
        protected override CompositeKey CalculateKey(IInvocation invocation, string[] keyAttributes = null)
        {
            try
            {
                CompositeKey result = new CompositeKey();
                List<object> argumentsForKey = null;

                argumentsForKey = new List<object>() { invocation.Method.Name };
                result.MinimalKey = invocation.Method.Name;

                if (keyAttributes != null && keyAttributes.Length > 0)
                {
                    // note: not expanding parameters of type list/array - the key may be longer than the actual data
                    argumentsForKey.AddRange(invocation.Method.GetParameters().Where(p => keyAttributes.Contains(p.Name)).Select(
                        p => invocation.GetArgumentValue(p.Position)));
                }

                result.FullKey = string.Join("-", argumentsForKey);
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected override ICacheManager Cache
        {
            get { return ClientFactory.GetClient<ICacheManager>(); }
        }

    }
}
