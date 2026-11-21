using System;
using System.Collections.Generic;

namespace AmplaData.Web.Wrappers
{
    public class SimpleHttpResponse : IHttpResponseWrapper, IDisposable
    {
        public static SimpleHttpResponse Create(Action<string> redirectFunc)
        {
            return new SimpleHttpResponse(redirectFunc);
        }

        private readonly Action<string> redirectFunc;
        private readonly Dictionary<string, string> cookies;
        private bool disposed;

        private SimpleHttpResponse(Action<string> redirectFunc)
        {
            this.redirectFunc = redirectFunc;
            cookies = new Dictionary<string, string>();
            disposed = false;
        }

        public IReadOnlyDictionary<string, string> Cookies
        {
            get
            {
                CheckDisposed();
                return cookies;
            }
        }

        public void AppendCookie(string name, string value, TimeSpan? expires = null)
        {
            CheckDisposed();
            cookies[name] = value;
        }

        public void DeleteCookie(string name)
        {
            CheckDisposed();
            cookies.Remove(name);
        }

        public void Redirect(string url)
        {
            CheckDisposed();
            redirectFunc(url);
        }

        public void Dispose()
        {
            disposed = true;
        }

        private void CheckDisposed()
        {
            if (disposed)
                throw new ObjectDisposedException("HttpResponse");
        }
    }
}