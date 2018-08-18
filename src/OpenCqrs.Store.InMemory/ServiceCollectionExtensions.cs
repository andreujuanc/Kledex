using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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

            builder.Services.Scan(s => s
                .FromAssembliesOf(typeof(InMemoryStore))
                .AddClasses()
                .AsImplementedInterfaces());

            return builder;
        }
    }
}
