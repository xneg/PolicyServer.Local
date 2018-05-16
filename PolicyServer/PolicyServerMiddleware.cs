using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace PolicyServer
{
    public class PolicyServerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IEndpointRouter _endpointRouter;
        private readonly ILogger _logger;

        public PolicyServerMiddleware(RequestDelegate next, IEndpointRouter endpointRouter, ILogger<PolicyServerMiddleware> logger)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _endpointRouter = endpointRouter ?? throw new ArgumentNullException(nameof(endpointRouter));
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var endPoint = _endpointRouter.Find(context);
            if (endPoint != null)
            {
                var result = await endPoint.ProcessAsync(context);

                if (result != null)
                {
                    await result.ExecuteAsync(context);
                }

                return;
            }

            await _next(context);
        }
    }
}
