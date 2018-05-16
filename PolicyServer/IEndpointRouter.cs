using Microsoft.AspNetCore.Http;

namespace PolicyServer
{
    /// <summary>
    /// The endpoint router
    /// </summary>
    public interface IEndpointRouter
    {
        /// <summary>
        /// Finds a matching endpoint.
        /// </summary>
        /// <param name="context">The HTTP context.</param>
        /// <returns></returns>
        IEndpoint Find(HttpContext context);
    }
}
