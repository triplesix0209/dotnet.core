using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TripleSix.Core.WebApi.Controllers;
using TripleSix.Core.WebApi.Results;

namespace Sample.Controllers
{
    public class TestController : BaseController
    {
        [HttpGet]
        [SwaggerOperation("Test")]
        [SwaggerResponse(200, null, typeof(SuccessResult))]
        public Task<IActionResult> GetList()
        {
            return Task.FromResult<IActionResult>(SuccessResult());
        }
    }
}
