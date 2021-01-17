using System;
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
        [Route("exceptions/server-error")]
        public IActionResult GetException()
        {
            throw new Exception();
        }
    }
}
