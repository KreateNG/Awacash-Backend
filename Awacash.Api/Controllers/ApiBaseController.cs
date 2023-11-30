using Awacash.Infrastructure.Filters;
using ErrorOr;
using Microsoft.AspNetCore.Mvc;

namespace Awacash.Api.Controllers
{
    //[Route("api/[controller]")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    //[TypeFilter(typeof(ApiAuthorizationActionFilter))]
    public class ApiBaseController : ControllerBase
    {
        protected IActionResult Problem(List<Error> errors)
        {
            var firstError = errors[0];

            var statusCode = firstError.Type switch
            {
                ErrorType.Conflict => StatusCodes.Status409Conflict,
                ErrorType.NotFound => StatusCodes.Status404NotFound,
                ErrorType.Validation => StatusCodes.Status400BadRequest,
                _ => StatusCodes.Status500InternalServerError
            };
            return Problem(statusCode: statusCode, title: firstError.Description);
        }
    }
}
