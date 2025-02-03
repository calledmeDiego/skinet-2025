using SKYNETAPI.Errors;
using System.Net;
using System.Text.Json;

namespace SKYNETAPI.Middleware;

public class ExceptionMiddleware(IHostEnvironment env, RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
		try
		{
			await next(context);
		}
		catch (Exception ex)
		{
			await HandleExceptionAsync(context, ex, env);
		}
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception ex, IHostEnvironment environment)
    {
        var responseContext = context.Response;

        responseContext.ContentType = "application/json";
        responseContext.StatusCode = (int)HttpStatusCode.InternalServerError;

        var response = environment.IsDevelopment() ? new ApiErrorResponse(responseContext.StatusCode, ex.Message, ex.StackTrace) : 
            new ApiErrorResponse(responseContext.StatusCode, ex.Message, "Internal server error");

        var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

        var json = JsonSerializer.Serialize(response, options);

        return context.Response.WriteAsync(json);

    }
}
