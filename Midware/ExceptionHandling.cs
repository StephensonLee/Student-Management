using System.Net;

namespace Student_Management.Midware
{
    public class ExceptionHandling
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public ExceptionHandling(RequestDelegate next) => _next = next;

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception e)
            {
                _logger.LogError(e,e.Message);
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }

        }
    }
}
