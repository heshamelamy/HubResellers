using Autofac;
using Autofac.Integration.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Util
{
    public class AutoFacBootstrap
    {
        public static void Configure(ContainerBuilder builder)
        {
            builder.RegisterControllers(Assembly.GetCallingAssembly());
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            builder.RegisterAssemblyTypes(assemblies).Where(t => t.Name.EndsWith("Service")).AsImplementedInterfaces().InstancePerRequest();
            builder.RegisterAssemblyTypes(assemblies).Where(t => t.Name.EndsWith("Repository")).AsImplementedInterfaces().InstancePerRequest();
            builder.RegisterAssemblyTypes(assemblies).Where(t => t.Name.EndsWith("Entities")).InstancePerRequest();
            builder.RegisterAssemblyTypes(assemblies).Where(t => t.Name.EndsWith("Factory")).AsImplementedInterfaces().InstancePerRequest();

            IContainer container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }

    }
}
