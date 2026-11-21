using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web;

namespace AmplaData.Web.Wrappers
{
    public class SimpleHttpRequest : IHttpRequestWrapper, IDisposable
    {
        private bool disposed;
        private readonly bool isAuthenticated;
        private readonly Dictionary<string, string> cookies;
        private readonly Uri url;
        private readonly NameValueCollection queryString;

        public static SimpleHttpRequest Create(
            string requestUrl,
            IReadOnlyDictionary<string, string> cookies,
            bool isAuthenticated)
        {
            return new SimpleHttpRequest(requestUrl, cookies, isAuthenticated);
        }

        private SimpleHttpRequest(
            string requestUrl,
            IReadOnlyDictionary<string, string> cookies,
            bool isAuthenticated)
        {
            this.isAuthenticated = isAuthenticated;
            url = new Uri(requestUrl);

            queryString = HttpUtility.ParseQueryString(url.Query);

            this.cookies = cookies != null
                ? new Dictionary<string, string>(cookies)
                : new Dictionary<string, string>();

            disposed = false;
        }

        public NameValueCollection QueryString
        {
            get
            {
                CheckDisposed();
                return queryString;
            }
        }

        public Uri Url
        {
            get
            {
                CheckDisposed();
                return url;
            }
        }

        public IReadOnlyDictionary<string, string> Cookies
        {
            get
            {
                CheckDisposed();
                return cookies;
            }
        }

        public string GetCookie(string name)
        {
            CheckDisposed();

            string value;
            return cookies.TryGetValue(name, out value)
                ? value
                : null;
        }

        public bool IsAuthenticated
        {
            get
            {
                CheckDisposed();
                return isAuthenticated;
            }
        }

        public void Dispose()
        {
            disposed = true;
        }

        private void CheckDisposed()
        {
            if (disposed)
                throw new ObjectDisposedException("HttpRequest");
        }
    }
}