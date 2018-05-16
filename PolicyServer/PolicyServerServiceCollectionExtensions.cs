using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace PolicyServer
{
    /// <summary>
    /// DI extension methods.
    /// </summary>
    public static class PolicyServerServiceCollectionExtensions
    {
        public static IServiceCollection AddPolicyServer(this IServiceCollection services)
        {
            services.AddSingleton<IEndpointRouter>(resolver =>
                new EndpointRouter(
                    Constants.EndpointPathToNameMap,
                    resolver.GetServices<EndpointMapping>(),
                    resolver.GetRequiredService<ILogger<EndpointRouter>>()));

            return services;
        }
    }
}
