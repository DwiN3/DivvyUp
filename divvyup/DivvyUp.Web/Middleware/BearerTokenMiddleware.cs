namespace DivvyUp.Web.Middleware
{
    public class BearerTokenMiddleware
    {
        private readonly RequestDelegate _next;

        public BearerTokenMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Headers.ContainsKey("Authorization"))
            {
                var token = context.Request.Headers["Authorization"].ToString();
                if (!token.StartsWith("Bearer "))
                {
                    context.Request.Headers["Authorization"] = "Bearer " + token;
                }
            }

            await _next(context);
        }
    }
}
