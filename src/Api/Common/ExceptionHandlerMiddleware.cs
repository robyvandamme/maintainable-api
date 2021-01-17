using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Serilog;

namespace Maintainable.Api.Common
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context).ConfigureAwait(false);
            }
            catch (Exception exception)
            {
                var errorId = Guid.NewGuid().ToString();

                Log.ForContext<ExceptionHandlerMiddleware>().Error(
                    exception,
                    "Exception caught at {path} {query} with errorId {errorId}",
                    context.Request.Path,
                    context.Request.Query,
                    errorId);

                var problemDetails = new ServerErrorProblemDetails(
                    errorId,
                    exception.Message,
                    exception.StackTrace);
                var errorResult = new ServerErrorResult(problemDetails);
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await context.Response.WriteAsJsonAsync(errorResult);
            }
        }
    }
}
