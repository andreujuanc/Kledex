using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenCqrs.Bus;
using OpenCqrs.Dependencies;
using OpenCqrs.Domain;
using OpenCqrs.Extensions;
using System;

namespace OpenCqrs.Store.InMemory
{
    public static class ServiceCollectionExtensions
    {
        public static IOpenCqrsServiceBuilder AddInMemoryProvider(this IOpenCqrsServiceBuilder builder, IConfiguration configuration)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));

            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));

            var store = new InMemoryStore(new VersionService());// todo
            var busdistpacher = new InMemoryBusMessageDispatcher();

            builder.Services.AddSingleton<IEventStore>((sp) => store);
            builder.Services.AddSingleton<ICommandStore>((sp) => store);
            builder.Services.AddSingleton<IBusMessageDispatcher>((sp) => busdistpacher);
            builder.Services.AddSingleton<IResolver, Resolver>();
            

            builder.Services.Scan(s => s
                .FromAssembliesOf(typeof(InMemoryStore))
                .AddClasses()
                .AsImplementedInterfaces());

            return builder;
        }
    }
}
