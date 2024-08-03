namespace Sample.Application.Dto.Admins
{
    public class SiteFilterAdminDto : BaseFilterAdminDto<Site>
    {
        public string? Code { get; set; }

        public string? Name { get; set; }

        /// <inheritdoc/>
        public override Task<IQueryable<Site>> ToQueryable(IQueryable<Site> query, IServiceProvider serviceProvider)
        {
            var result = query
                .WhereIf(Code.IsNotNullOrEmpty(), x => EF.Functions.Like(x.Code, $"%{Code}%"))
                .WhereIf(Name.IsNotNullOrEmpty(), x => EF.Functions.Like(x.Name, $"%{Name}%"));

            return Task.FromResult(result);
        }
    }
}
