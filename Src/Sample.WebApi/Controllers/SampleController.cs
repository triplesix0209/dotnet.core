using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Sample.WebApi.Abstracts;
using TripleSix.Core.Dto;

namespace Sample.WebApi.Controllers
{
    public class SampleController : CommonController
    {
        [HttpGet]
        public async Task<IActionResult> Test()
        {
            var identity = GenerateIdentity<IIdentity>();
            return SuccessResult();
        }
    }
}
