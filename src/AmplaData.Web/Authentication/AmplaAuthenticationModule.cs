using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using AmplaData.Web.Sessions;

namespace AmplaData.Web.Authentication
{
    /// <summary>
    /// Ampla Authentication Middleware for modern .NET 10 pipelines
    /// </summary>
    public class AmplaAuthenticationMiddleware
    {
        private readonly RequestDelegate _next;

        public AmplaAuthenticationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // 1. Authenticate Request equivalent: Check for AmplaSession in query string if not authenticated
            if (context.User?.Identity?.IsAuthenticated != true)
            {
                var loginAmplaSession = context.RequestServices.GetService<LoginAmplaSessionUsingQueryString>();
                loginAmplaSession?.Execute();
            }

            // Continue down the request pipeline (this handles controllers, views, etc.)
            await _next(context);

            // 2. Post Acquire Request State equivalent: Ensure session is aligned with authentication
            var ensureAlignedSession = context.RequestServices.GetService<AlignSessionWithFormsAuthentication>();
            ensureAlignedSession?.Execute();
        }
    }
}