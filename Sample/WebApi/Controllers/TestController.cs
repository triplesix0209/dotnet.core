using System.Net.Http.Headers;
using System.Text;

namespace Sample.WebApi.Controllers.Admins
{
    [SwaggerTag("Test")]
    public class TestController : CommonController
    {
        [HttpGet]
        [SwaggerOperation("Test")]
        public async Task<SuccessResult> Test()
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                "Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes("dmcl.integration:DmclS4@2022")));
            var response = await client.PostAsync($"https://sapapipopsrv53.stdmcl.com:53443/RESTAdapter/POS/GetSites", new StringContent(
                "{}", Encoding.UTF8, "application/json"));

            return SuccessResult();
        }
    }
}
