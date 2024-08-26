using LibraryBook.Areas.Data;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using LibraryBook.Data;
using Microsoft.AspNetCore.Builder;

namespace LibraryBook.Middlewares
{
    public class UserTableMiddleware
    {
        private readonly RequestDelegate _next;

        public UserTableMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, ApplicationDbContext dbContext)
        {
            // Expose the Users table via HttpContext
            context.Items["UsersTable"] = dbContext.Users;

            await _next(context);
        }
    }

    // Extension method to add the middleware to the pipeline
    public static class UserTableMiddlewareExtensions
    {
        public static IApplicationBuilder UseUserTableMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<UserTableMiddleware>();
        }
    }
}
