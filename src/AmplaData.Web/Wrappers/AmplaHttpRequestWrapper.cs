using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using Microsoft.AspNetCore.Http;

namespace AmplaData.Web.Wrappers
{
    /// <summary>
    /// Wrapper class for HttpRequest.
    /// </summary>
    public class AmplaHttpRequestWrapper : IHttpRequestWrapper
    {
        private readonly HttpRequest request;

        public AmplaHttpRequestWrapper(HttpRequest request)
        {
            this.request = request;
        }

        public NameValueCollection QueryString
        {
            get
            {
                NameValueCollection collection = new NameValueCollection();

                foreach (var key in request.Query.Keys)
                {
                    collection[key] = request.Query[key];
                }

                return collection;
            }
        }

        public Uri Url
        {
            get
            {
                return new Uri(
                    $"{request.Scheme}://{request.Host}{request.Path}{request.QueryString}");
            }
        }

        public IReadOnlyDictionary<string, string> Cookies
        {
            get
            {
                var cookies = new Dictionary<string, string>();

                foreach (var cookie in request.Cookies)
                {
                    cookies[cookie.Key] = cookie.Value;
                }

                return cookies;
            }
        }

        public string GetCookie(string name)
        {
            request.Cookies.TryGetValue(name, out string value);
            return value;
        }

        public bool IsAuthenticated
        {
            get
            {
                return request.HttpContext.User?.Identity?.IsAuthenticated ?? false;
            }
        }
    }
}