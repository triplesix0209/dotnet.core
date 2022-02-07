using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Sample.Dto;
using TripleSix.Core.Attributes;
using TripleSix.Core.WebApi.Controllers;
using TripleSix.Core.WebApi.Results;

namespace Sample.Controllers
{
    public class TestController : BaseController
    {
        [HttpGet]
        [SwaggerApi(typeof(DataResult<string>))]
        public Task<IActionResult> Get(InputDto input)
        {
            input.Validate();
            var result = DataResult(input);
            return Task.FromResult<IActionResult>(result);
        }
    }
}
