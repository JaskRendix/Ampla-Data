using System;

namespace AmplaData.Web.Authentication.Forms
{
    /// <summary>
    /// Replacement for the old FormsAuthenticationTicket.
    /// </summary>
    public class AuthenticationTicket
    {
        public string UserName { get; set; }

        public string Session { get; set; }

        // Added to replace FormsAuthenticationTicket.UserData
        public string UserData { get; set; }

        public bool IsPersistent { get; set; }

        public DateTime IssueDate { get; set; }

        public DateTime Expiration { get; set; }
    }
}