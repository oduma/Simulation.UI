using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.Windsor;
using Castle.Windsor.Configuration.Interpreters;

namespace Sciendo.Core
{
    public static class ClientFactory
    {
        private static IWindsorContainer _container =  new WindsorContainer(new XmlInterpreter());

        public static T GetClient<T>()
        {
            try
            {
                return _container.Resolve<T>();
            }
            catch
            {
                throw new NotImplementedException();
            }
        }
    }
}
