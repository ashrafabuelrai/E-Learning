using E_Learning.Domain.Entities;
using E_Learning.Domain.Enums;

namespace E_Learning.API.Middleware
{
    public class UserContextMiddleware
    {
        private readonly RequestDelegate _next;

        public UserContextMiddleware(RequestDelegate next) => _next = next;

        public async Task InvokeAsync(HttpContext context)
        {
            var userContext = new UserContext();

            if (context.Request.Headers.TryGetValue("X-User-Role", out var role))
            {
                if(role.ToString()=="Admin") userContext.Role = UserRole.Admin;
                else if(role.ToString()=="Learner") userContext.Role = UserRole.Learner;
                else if(role.ToString()=="Manager") userContext.Role = UserRole.Manager;
            }

            if (context.Request.Headers.TryGetValue("X-User-Id", out var userIdStr) &&
                Guid.TryParse(userIdStr, out var userId))
                userContext.UserId = userId;

            context.Items["UserContext"] = userContext;

            await _next(context);
        }
    }

    public static class UserContextMiddlewareExtensions
    {
        public static IApplicationBuilder UseUserContextMiddleware(this IApplicationBuilder app)
            => app.UseMiddleware<UserContextMiddleware>();
    }
}
