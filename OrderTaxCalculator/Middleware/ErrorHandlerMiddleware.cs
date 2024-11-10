
using System.Text.Json;
using System.Text.Json.Serialization;

namespace OrderTaxCalculator.Middleware
{
    public class ErrorHandlerMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex) { 
             await HandleExceptionAsync(context, ex);


            }
        }

        private static async Task HandleExceptionAsync(HttpContext httpContext,Exception exception)
        {
            httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
            httpContext.Response.ContentType = "application/json";
            await httpContext.Response.WriteAsync(JsonSerializer.Serialize(exception.Message));

        }
    }
}
