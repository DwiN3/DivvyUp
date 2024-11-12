using DivvyUp_Impl_Maui.Api.Exceptions;
using Newtonsoft.Json;
using System.Net;

namespace DivvyUp.Web.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (DException ex)
            {
                await HandleDExceptionAsync(context, ex);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleDExceptionAsync(HttpContext context, DException ex)
        {
            context.Response.ContentType = "text/plain";
            context.Response.StatusCode = (int)ex.Status;

            return context.Response.WriteAsync(ex.Message);
        }

        private Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "text/plain";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            return context.Response.WriteAsync("An unexpected error occurred");
        }

    }
}
