namespace Sample.WebApi.Controllers.Commons
{
    [SwaggerTag("Test")]
    public class TestController : CommonController
    {
        [HttpGet]
        [SwaggerOperation("Test")]
        [Consumes("multipart/form-data")]
        public async Task<SuccessResult> Test(FileInput input)
        {
            return SuccessResult();
        }
    }
}
