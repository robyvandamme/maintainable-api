using System;
using Maintainable.Api.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Maintainable.Api.Controllers
{
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [ApiController]
    [Route("tests")]
    public class TestController : ControllerBase
    {
        [HttpGet]
        [Route("errors/server-error")]
        public IActionResult GetException()
        {
            throw new Exception();
        }

        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet]
        [Route("errors/not-found")]
        public IActionResult GetNotFound()
        {
            throw new NotFoundException();
        }
    }
}
