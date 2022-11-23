using Api.Services;

namespace Api.Middlewares
{
    public class StopDdosMiddleware
    {
        private readonly RequestDelegate _next;

        public StopDdosMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, DdosGuard guard)
        {
            var headerAuth = context.Request.Headers.Authorization;

            try
            {
                guard.CheckRequest(headerAuth);
                await _next(context);
            }
            catch(TooManyRequestException)
            {
                context.Response.StatusCode = 429;
                await context.Response.WriteAsJsonAsync("Too many requests, allowed 10 requests per second");
            }
        }
    }

    public static class StopDdosMiddlewareMiddlewareExtensions
    {
        public static IApplicationBuilder UseAntiDdosCustom(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<StopDdosMiddleware>();
        }
    }
}
