using System;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace AmplaData.Web.Wrappers
{
    /// <summary>
    /// Interface that wraps the HttpRequest object to simplify testing.
    /// </summary>
    public interface IHttpRequestWrapper
    {
        /// <summary>
        /// Gets the query string for the request.
        /// </summary>
        NameValueCollection QueryString { get; }

        /// <summary>
        /// Gets the URL for the request.
        /// </summary>
        Uri Url { get; }

        /// <summary>
        /// Gets whether the request is authenticated.
        /// </summary>
        bool IsAuthenticated { get; }

        /// <summary>
        /// Gets a cookie value.
        /// </summary>
        string GetCookie(string name);

        /// <summary>
        /// Gets all cookies.
        /// </summary>
        IReadOnlyDictionary<string, string> Cookies { get; }
    }
}