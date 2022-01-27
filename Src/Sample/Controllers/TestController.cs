using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Sample.Dto;
using Swashbuckle.AspNetCore.Annotations;
using TripleSix.Core.WebApi.Controllers;
using TripleSix.Core.WebApi.Filters;
using TripleSix.Core.WebApi.Results;

namespace Sample.Controllers
{
    public class TestController : BaseController
    {
        [HttpGet]
        [SwaggerOperation("Test")]
        [SwaggerResponse(200, null, typeof(DataResult<string>))]
        [ValidateModel]
        public Task<IActionResult> Test([FromBody] InputDto input)
        {
            var result = DataResult(input.Data);
            return Task.FromResult<IActionResult>(result);
        }
    }
}
