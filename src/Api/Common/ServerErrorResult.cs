using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Maintainable.Api.Common
{
    public class ServerErrorResult : ObjectResult
    {
        public ServerErrorResult(ServerErrorProblemDetails serverErrorProblemDetails)
            : base(serverErrorProblemDetails)
        {
            StatusCode = StatusCodes.Status500InternalServerError;
        }
    }
}
