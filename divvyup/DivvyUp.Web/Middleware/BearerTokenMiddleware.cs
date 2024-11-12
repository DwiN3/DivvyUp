namespace DivvyUp.Web.Middleware
{
    public class BearerTokenMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            if (context.Request.Headers.ContainsKey("Authorization"))
            {
                var token = context.Request.Headers["Authorization"].ToString();
                if (!token.StartsWith("Bearer "))
                {
                    context.Request.Headers["Authorization"] = "Bearer " + token;
                }
            }

            await next(context);
        }
    }
}
