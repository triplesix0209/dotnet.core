using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Sample.Common;
using Sample.WebApi.Abstracts;

namespace Sample.WebApi.Controllers
{
    public class SampleController : CommonController
    {
        [HttpGet]
        public async Task<IActionResult> Test()
        {
            var identity = GenerateIdentity<Identity>();
            return SuccessResult();
        }
    }
}
