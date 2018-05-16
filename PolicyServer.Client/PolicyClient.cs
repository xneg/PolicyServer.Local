using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace PolicyServer.Client
{
    public class PolicyClient : IDisposable
    {
        private bool _disposed;

        /// <summary>The HTTP client</summary>
        protected HttpClient _client;

        public PolicyClient(string address)
            : this(address, new HttpClientHandler())
        {
        }

        public PolicyClient(string address, HttpMessageHandler innerHttpMessageHandler)
        {
            if (address == null) throw new ArgumentNullException(nameof(address));
            if (innerHttpMessageHandler == null) throw new ArgumentNullException(nameof(innerHttpMessageHandler));

            _client = new HttpClient(innerHttpMessageHandler)
            {
                BaseAddress = new Uri(address)
            };

            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            // AuthenticationStyle = AuthenticationStyle.None;
        }

        // public AuthenticationStyle AuthenticationStyle { get; set; }


        public virtual async Task<PolicyResponse> RequestAsync(
            IDictionary<string, string> form,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var response = await _client.PostAsync(string.Empty, new FormUrlEncodedContent(form), cancellationToken).ConfigureAwait(false);

            if (response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.BadRequest)
            {
                var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                return new PolicyResponse(content);
            }
            else
            {
                return new PolicyResponse(response.StatusCode, response.ReasonPhrase);
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing && !_disposed)
            {
                _disposed = true;
                _client.Dispose();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
