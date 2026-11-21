using System;

namespace AmplaData.Web.Wrappers
{
    /// <summary>
    /// Interface that wraps the HttpResponse object to simplify testing.
    /// </summary>
    public interface IHttpResponseWrapper
    {
        /// <summary>
        /// Redirects the specified URL.
        /// </summary>
        void Redirect(string url);

        /// <summary>
        /// Appends a cookie.
        /// </summary>
        void AppendCookie(
            string name,
            string value,
            TimeSpan? expiration = null);

        /// <summary>
        /// Deletes a cookie.
        /// </summary>
        void DeleteCookie(string name);
    }
}