using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Sample.Dto;
using TripleSix.Core.WebApi.Controllers;

namespace Sample.Controllers
{
    public class TestController : BaseController
    {
        [HttpGet]
        public Task<IActionResult> Test([FromBody] InputDto input)
        {
            input.Validate();
            var result = DataResult(input);
            return Task.FromResult<IActionResult>(result);
        }
    }
}
