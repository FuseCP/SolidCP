using System.Web.Http.Dependencies;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SolidCP.WebDavPortal.DependencyInjection
{
    public class NinjectDependecyResolver : System.Web.Mvc.IDependencyResolver, System.Web.Http.Dependencies.IDependencyResolver
    {
        IKernel kernal;

        public NinjectDependecyResolver()
        {
            kernal = new StandardKernel(new NinjectSettings { AllowNullInjection = true });

            AddBindings();
        }

        public object GetService(Type serviceType)
        {
            return kernal.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return kernal.GetAll(serviceType);
        }

        public IDependencyScope BeginScope()
        {
            return this;
        }

        private void AddBindings()
        {
            PortalDependencies.Configure(kernal);
        }

        public void Dispose()
        {
            
        }
    }
}