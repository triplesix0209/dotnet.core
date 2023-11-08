namespace Sample.WebApi.Controllers.Admins
{
    public class AdminReadEndpoint<TEntity, TData, TFilter> : AdminController
        where TEntity : class, IStrongEntity
        where TData : class, IDto
        where TFilter : class, IQueryableDto<TEntity>
    {
        public IStrongService<TEntity> Service { get; set; }

        [HttpGet]
        [SwaggerOperation("Lấy danh sách")]
        public async Task<PagingResult<TData>> GetPage(TFilter input)
        {
            var result = await Service.GetPageByQueryModel<TData>(input, 1, 10);
            return PagingResult(result);
        }
    }
}