namespace UserManagementAPI.Middleware
{
    public class AuthenticationMiddleware
    {
        private readonly RequestDelegate _next;

        private const string ApiToken =
            "TechHiveSecureToken123";

        public AuthenticationMiddleware(
            RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(
            HttpContext context)
        {
            if (!context.Request.Headers
                .TryGetValue("Authorization",
                out var token))
            {
                context.Response.StatusCode = 401;

                await context.Response.WriteAsJsonAsync(
                    new
                    {
                        error = "Authorization token is missing."
                    });

                return;
            }

            var providedToken =
                token.ToString().Replace("Bearer ", "");

            if (providedToken != ApiToken)
            {
                context.Response.StatusCode = 401;

                await context.Response.WriteAsJsonAsync(
                    new
                    {
                        error = "Invalid token."
                    });

                return;
            }

            await _next(context);
        }
    }
}