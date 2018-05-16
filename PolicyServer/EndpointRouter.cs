using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace PolicyServer
{
    public class EndpointRouter : IEndpointRouter
    {
        private readonly Dictionary<string, EndpointName> _pathToNameMap;
        private readonly IEnumerable<EndpointMapping> _mappings;
        private readonly ILogger<EndpointRouter> _logger;

        public EndpointRouter(Dictionary<string, EndpointName> pathToNameMap, IEnumerable<EndpointMapping> mappings, ILogger<EndpointRouter> logger)
        {
            _pathToNameMap = pathToNameMap ?? throw new ArgumentNullException(nameof(pathToNameMap));
            _mappings = mappings ?? throw new ArgumentNullException(nameof(mappings));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public IEndpoint Find(HttpContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            foreach (var key in _pathToNameMap.Keys)
            {
                var path = "/" + key;
                if (context.Request.Path.StartsWithSegments(path))
                {
                    var endpointName = _pathToNameMap[key];
                    return GetEndpoint(endpointName, context);
                }
            }

            return null;
        }

        private IEndpoint GetEndpoint(EndpointName endpointName, HttpContext context)
        {
            /*if (_options.Endpoints.IsEndpointEnabled(endpointName))
            {*/
                var mapping = _mappings.LastOrDefault(x => x.Endpoint == endpointName);
                if (mapping != null)
                {
                    _logger.LogDebug("Mapping found for endpoint: {endpoint}, creating handler: {endpointHandler}", endpointName, mapping.Handler.FullName);
                    return context.RequestServices.GetService(mapping.Handler) as IEndpoint;
                }
                else
                {
                    _logger.LogError("No mapping found for endpoint: {endpoint}", endpointName);
                }
            /*}
            else
            {
                _logger.LogWarning("{endpoint} endpoint requested, but is disabled in endpoint options.", endpointName);
            }*/

            return null;
        }
    }
}
