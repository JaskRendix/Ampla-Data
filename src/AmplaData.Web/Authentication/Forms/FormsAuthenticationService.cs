using System;
using System.Text;
using System.Text.Json;
using AmplaData.Web.Wrappers;

namespace AmplaData.Web.Authentication.Forms
{
    /// <summary>
    /// Service that wraps authentication logic using modern .NET primitives.
    /// </summary>
    public class FormsAuthenticationService : IFormsAuthenticationService
    {
        private readonly IHttpRequestWrapper request;
        private readonly IHttpResponseWrapper response;

        private const string CookieName = ".AmplaAuth";

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="FormsAuthenticationService"/> class.
        /// </summary>
        public FormsAuthenticationService(
            IHttpRequestWrapper request,
            IHttpResponseWrapper response)
        {
            this.request = request;
            this.response = response;
        }

        /// <summary>
        /// Sign out the current user.
        /// </summary>
        public void SignOut()
        {
            response.DeleteCookie(CookieName);
        }

        /// <summary>
        /// Expire the current session.
        /// </summary>
        public void SessionExpired()
        {
            SignOut();

            string url = request.Url?.ToString() ?? "/";

            response.Redirect(url);
        }

        /// <summary>
        /// Stores the user ticket.
        /// </summary>
        public void StoreUserTicket(
            AmplaUser amplaUser,
            bool createPersistentCookie)
        {
            var ticket = new AuthenticationTicket
            {
                UserName = amplaUser.UserName,
                Session = amplaUser.Session,
                UserData = amplaUser.Session,
                IsPersistent = createPersistentCookie,
                IssueDate = DateTime.UtcNow,
                Expiration = DateTime.UtcNow.AddMinutes(30)
            };

            string jsonTicket =
                JsonSerializer.Serialize(ticket);

            string encodedTicket =
                Convert.ToBase64String(
                    Encoding.UTF8.GetBytes(jsonTicket));

            response.AppendCookie(
                CookieName,
                encodedTicket,
                createPersistentCookie
                    ? TimeSpan.FromDays(7)
                    : (TimeSpan?)null);
        }

        /// <summary>
        /// Gets the user ticket.
        /// </summary>
        public AuthenticationTicket GetUserTicket()
        {
            var authCookieValue =
                request.GetCookie(CookieName);

            if (!string.IsNullOrEmpty(authCookieValue))
            {
                try
                {
                    byte[] data =
                        Convert.FromBase64String(
                            authCookieValue);

                    string jsonTicket =
                        Encoding.UTF8.GetString(data);

                    return JsonSerializer.Deserialize<AuthenticationTicket>(
                        jsonTicket);
                }
                catch
                {
                    // Ignore malformed cookies.
                }
            }

            return null;
        }
    }
}