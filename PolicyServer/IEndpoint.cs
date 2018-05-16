using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace PolicyServer
{
    /// <summary>
    /// Endpoint handler
    /// </summary>
    public interface IEndpoint
    {
        /// <summary>
        /// Processes the request.
        /// </summary>
        /// <param name="context">The HTTP context.</param>
        /// <returns></returns>
        Task<IEndpointResult> ProcessAsync(HttpContext context);
    }
}
