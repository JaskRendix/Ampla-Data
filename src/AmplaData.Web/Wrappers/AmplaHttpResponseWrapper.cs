using System;
using Microsoft.AspNetCore.Http;

namespace AmplaData.Web.Wrappers
{
    /// <summary>
    /// Wrapper class for HttpResponse.
    /// </summary>
    public class AmplaHttpResponseWrapper : IHttpResponseWrapper
    {
        private readonly HttpResponse response;

        public AmplaHttpResponseWrapper(HttpResponse response)
        {
            this.response = response;
        }

        public void Redirect(string url)
        {
            response.Redirect(url);
        }

        public void AppendCookie(
            string name,
            string value,
            TimeSpan? expiration = null)
        {
            CookieOptions options = new CookieOptions();

            if (expiration.HasValue)
            {
                options.Expires =
                    DateTimeOffset.UtcNow.Add(expiration.Value);
            }

            response.Cookies.Append(
                name,
                value,
                options);
        }

        public void DeleteCookie(string name)
        {
            response.Cookies.Delete(name);
        }
    }
}