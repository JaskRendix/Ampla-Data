using System.Text.Json;
using Microsoft.AspNetCore.Http;

namespace AmplaData.Web.Wrappers
{
    /// <summary>
    /// Wrapper class for the HttpSession
    /// </summary>
    public class AmplaHttpSessionWrapper : IHttpSessionWrapper
    {
        private readonly ISession session;

        /// <summary>
        /// Initializes a new instance of the <see cref="AmplaHttpSessionWrapper"/> class.
        /// </summary>
        /// <param name="session">The HTTP session.</param>
        public AmplaHttpSessionWrapper(ISession session)
        {
            this.session = session;
        }

        /// <summary>
        /// Gets a value indicating whether the session is [enabled].
        /// </summary>
        public bool Enabled
        {
            get { return session != null; }
        }

        /// <summary>
        /// Sets the value.
        /// </summary>
        public void SetValue(string key, object value)
        {
            if (session != null)
            {
                if (value == null)
                {
                    session.Remove(key);
                }
                else
                {
                    // Serialize object to JSON for storage in ASP.NET Core's byte[] session store
                    string json = JsonSerializer.Serialize(value);
                    session.SetString(key, json);
                }
            }
        }

        /// <summary>
        /// Gets the value.
        /// </summary>
        public object GetValue(string key)
        {
            if (session != null)
            {
                string json = session.GetString(key);
                if (!string.IsNullOrEmpty(json))
                {
                    return JsonSerializer.Deserialize<object>(json);
                }
            }
            return null;
        }
    }
}