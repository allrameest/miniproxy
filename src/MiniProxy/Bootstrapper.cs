using System;
using System.IO;
using Autofac;
using MiniProxy.Caching;
using Nancy.Bootstrappers.Autofac;
using Nancy.Json;

namespace MiniProxy
{
    public class Bootstrapper : AutofacNancyBootstrapper
    {
        protected override ILifetimeScope GetApplicationContainer()
        {
            var builder = new ContainerBuilder();
            builder
                .Register(_ => ReadProxyConfiguration())
                .As<ProxyConfiguration>()
                .SingleInstance();
            builder.Register(CreateCache).As<ICache>().SingleInstance();

            return builder.Build();
        }

        private static ProxyConfiguration ReadProxyConfiguration()
        {
            var configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.json");
            var content = File.ReadAllText(configPath);

            var serializer = new JavaScriptSerializer();
            return serializer.Deserialize<ProxyConfiguration>(content);
        }

        private static ICache CreateCache(IComponentContext ctx)
        {
            var config = ctx.Resolve<ProxyConfiguration>();
            if (config.EnableCaching)
            {
                return new FileCache();
            }

            return new NullCache();
        }
    }
}