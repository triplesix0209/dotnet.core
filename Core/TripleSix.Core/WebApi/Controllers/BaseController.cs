using Microsoft.AspNetCore.Mvc;

namespace TripleSix.Core.WebApi
{
    [Route("[controller]")]
    [Consumes("application/json")]
    [Produces("application/json")]
    public abstract class BaseController : ControllerBase
    {
    }
}
