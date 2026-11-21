using System;

namespace AmplaData.Web.Authentication.Forms
{
    /// <summary>
    /// Modern .NET 10 replacement for legacy System.Web.Security.FormsAuthenticationTicket
    /// </summary>
    public class FormsAuthenticationTicket
    {
        public string Name { get; set; }
        public string UserData { get; set; }
        public bool IsPersistent { get; set; }
        public DateTime Expiration { get; set; }
        public DateTime IssueDate { get; set; }

        public FormsAuthenticationTicket(string name, string userData, bool isPersistent, DateTime expiration)
        {
            Name = name;
            UserData = userData;
            IsPersistent = isPersistent;
            Expiration = expiration;
        }

        public FormsAuthenticationTicket() { }
    }
}