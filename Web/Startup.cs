using System;
using Autofac;
using Microsoft.Owin;
using Owin;
using HubManPractices.Service.ViewModels.Mappings;

[assembly: OwinStartupAttribute(typeof(WebApp.App_Start.Startup))]
namespace WebApp.App_Start
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            HubManPractices.Service.AuthStartup.Startup.ConfigureAuth(app);
            var builder = new ContainerBuilder();
            Util.AutoFacBootstrap.Configure(builder);
            AutoMapperConfiguration.Configure();
        }

    }
}
