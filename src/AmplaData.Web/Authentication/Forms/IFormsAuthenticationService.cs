namespace AmplaData.Web.Authentication.Forms
{
    /// <summary>
    /// Service that wraps authentication logic.
    /// </summary>
    public interface IFormsAuthenticationService
    {
        /// <summary>
        /// Sign out the current user.
        /// </summary>
        void SignOut();

        /// <summary>
        /// Expire the current session.
        /// </summary>
        void SessionExpired();

        /// <summary>
        /// Store user authentication information.
        /// </summary>
        /// <param name="amplaUser">The Ampla user.</param>
        /// <param name="createPersistentCookie">
        /// True to create a persistent cookie.
        /// </param>
        void StoreUserTicket(
            AmplaUser amplaUser,
            bool createPersistentCookie);

        /// <summary>
        /// Gets the current authentication ticket.
        /// </summary>
        AuthenticationTicket GetUserTicket();
    }
}