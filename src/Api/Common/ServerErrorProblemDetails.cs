using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Maintainable.Api.Common
{
    public class ServerErrorProblemDetails : ProblemDetails
    {
        public ServerErrorProblemDetails(string errorId, string message = null, string stackTrace = null)
        {
            Detail = $"Error ID: {errorId}";
            Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1";
            Status = StatusCodes.Status500InternalServerError;
            Title = "Internal server error";
            Extensions.Add("message", message);
            Extensions.Add("stackTrace", stackTrace);
        }
    }
}
