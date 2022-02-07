using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Sample.Common;
using TripleSix.Core.WebApi.Controllers;

namespace Sample.WebApi.Controllers
{
    public class SampleController : BaseController
    {
        [HttpGet]
        public async Task<IActionResult> Test()
        {
            var identity = GenerateIdentity<Identity>();
            return SuccessResult();
        }
    }
}
