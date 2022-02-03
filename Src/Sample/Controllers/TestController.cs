using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Sample.Dto;
using Swashbuckle.AspNetCore.Annotations;
using TripleSix.Core.WebApi.Controllers;
using TripleSix.Core.WebApi.Results;

namespace Sample.Controllers
{
    public class TestController : BaseController
    {
        [HttpGet]
        [SwaggerResponse(200, null, typeof(SuccessResult))]
        public Task<IActionResult> Test([FromBody] InputDto input)
        {
            input.Validate();
            var result = DataResult(input);
            return Task.FromResult<IActionResult>(result);
        }
    }
}
