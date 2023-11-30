
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Awacash.AdminApi.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    public class ApiBaseController : ControllerBase
    {

    }
}
