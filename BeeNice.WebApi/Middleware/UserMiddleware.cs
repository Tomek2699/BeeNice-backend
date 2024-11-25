using System.Security.Claims;

namespace BeeNice.WebApi.Middleware
{
    public class UserMiddleware
    {
        private readonly RequestDelegate _next;

        public UserMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId != null)
            {
                context.Items["UserId"] = userId;
            }

            await _next(context);
        }
    }
}
