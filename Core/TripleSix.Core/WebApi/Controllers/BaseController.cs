using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace TripleSix.Core.WebApi.Controllers
{
    [Route("[controller]")]
    [Consumes("application/json")]
    [Produces("application/json")]
    public abstract class BaseController : ControllerBase
    {
        /// <summary>
        /// Represents a set of key/value application configuration properties.
        /// </summary>
        public IConfiguration Configuration { get; set; }

        /// <summary>
        /// Automapper.
        /// </summary>
        public IMapper Mapper { get; set; }
    }
}
