using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Sample.Common.Dto;
using TripleSix.Core.Attributes;
using TripleSix.Core.WebApi.Controllers;
using TripleSix.Core.WebApi.Results;

namespace Sample.WebApi.Controllers
{
    public class SampleController : BaseController
    {
        [HttpGet]
        [SwaggerApi(typeof(DataResult<SampleDto>))]
        public async Task<IActionResult> Test()
        {
            return SuccessResult();
        }
    }
}
