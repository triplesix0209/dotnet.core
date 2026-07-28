namespace Sample.WebApi.Controllers.Commons
{
    [SwaggerTag("Healthcheck")]
    public class HealthcheckController : CommonController
    {
        [HttpGet]
        [SwaggerOperation("Healthcheck")]
        public Task<DataResult<string>> Healthcheck()
        {
            return Task.FromResult(DataResult("I am alive."));
        }
    }
}
