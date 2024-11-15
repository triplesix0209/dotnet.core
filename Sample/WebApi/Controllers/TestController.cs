using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TripleSix.Core.Helpers;

namespace Sample.WebApi.Controllers.Admins
{
    public class TestController : CommonController
    {
        [HttpGet]
        public async Task<SuccessResult> Test()
        {
            var sapHost = "https://sapapipopsrv53.stdmcl.com:53443/RESTAdapter";
            var sapKey = "dmcl.integration:DmclS4@2022";
            var method = "POS/GetSites?a=1";
            var input = new
            {
                REQUEST = "X",
            };

            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes(sapKey)));
            var requestContent = new StringContent(input == null ? "{}" : JsonConvert.SerializeObject(input), Encoding.UTF8, "application/json");
            var response = await client.PostAsync($"{sapHost}/{method}", requestContent);
            if (!response.IsSuccessStatusCode)
                throw new Exception(response.ReasonPhrase?.ToString(", ") ?? response.StatusCode.ToString());

            var responseContent = JObject.Parse(await response.Content.ReadAsStringAsync());

            return SuccessResult();
        }
    }
}
