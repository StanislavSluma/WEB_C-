using Microsoft.AspNetCore.Http.Extensions;
using Serilog;

namespace WEB_253505_Bekarev.Middleware
{
	public class LoggingRequestMiddleware
	{
		private readonly RequestDelegate _next;

		public LoggingRequestMiddleware(RequestDelegate next)
		{
			_next = next;
		}

		public async Task Invoke(HttpContext httpContext)
		{
			await _next(httpContext);

			if (httpContext.Response.StatusCode < 200 || httpContext.Response.StatusCode >= 300)
			{
				Log.Error($"request {httpContext.Request.GetDisplayUrl()} returns {httpContext.Response.StatusCode}");
			}
		}


	}

	public static class LogginRequestMiddlewareExtensions
	{
		public static IApplicationBuilder UseRequestLogginMiddleware(this IApplicationBuilder builder)
		{
			return builder.UseMiddleware<LoggingRequestMiddleware>();
		}
	}
}
