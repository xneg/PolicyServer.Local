using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Newtonsoft.Json.Linq;

namespace PolicyServer.Client
{
    public class PolicyResponse
    {
        public string Raw { get; protected set; }
        public JObject Json { get; protected set; }

        private bool _isHttpError;
        private HttpStatusCode _httpErrorstatusCode;
        private string _httpErrorReason;

        public PolicyResponse(string raw)
        {
            Raw = raw;

            try
            {
                Json = JObject.Parse(raw);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Invalid JSON response", ex);
            }
        }

        public PolicyResponse(HttpStatusCode statusCode, string reason)
        {
            _isHttpError = true;
            _httpErrorstatusCode = statusCode;
            _httpErrorReason = reason;
        }

        public bool IsHttpError => _isHttpError;

        public HttpStatusCode HttpErrorStatusCode => _httpErrorstatusCode;

        public string HttpErrorReason => _httpErrorReason;

        /*public string AccessToken
        {
            get
            {
                return GetStringOrNull(OidcConstants.TokenResponse.AccessToken);
            }
        }

        public string IdentityToken
        {
            get
            {
                return GetStringOrNull(OidcConstants.TokenResponse.IdentityToken);
            }
        }

        public string Error
        {
            get
            {
                return GetStringOrNull(OidcConstants.TokenResponse.Error);
            }
        }

        public bool IsError
        {
            get
            {
                return (IsHttpError ||
                        !string.IsNullOrWhiteSpace(GetStringOrNull(OidcConstants.TokenResponse.Error)));
            }
        }

        public long ExpiresIn
        {
            get
            {
                return GetLongOrNull(OidcConstants.TokenResponse.ExpiresIn);
            }
        }

        public string TokenType
        {
            get
            {
                return GetStringOrNull(OidcConstants.TokenResponse.TokenType);
            }
        }*/

        /*public string RefreshToken
        {
            get
            {
                return GetStringOrNull(OidcConstants.TokenResponse.RefreshToken);
            }
        }*/

        protected virtual string GetStringOrNull(string name)
        {
            JToken value;
            if (Json != null && Json.TryGetValue(name, StringComparison.OrdinalIgnoreCase, out value))
            {
                return value.ToString();
            }

            return null;
        }

        protected virtual long GetLongOrNull(string name)
        {
            JToken value;
            if (Json != null && Json.TryGetValue(name, out value))
            {
                long longValue = 0;
                if (long.TryParse(value.ToString(), out longValue))
                {
                    return longValue;
                }
            }

            return 0;
        }
    }
}
