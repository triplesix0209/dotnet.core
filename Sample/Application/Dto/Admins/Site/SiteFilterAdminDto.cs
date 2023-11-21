namespace Sample.Application.Dto.Admins
{
    public class SiteFilterAdminDto : BaseFilterAdminDto<Site>
    {
        public string? Code { get; set; }

        public string? Name { get; set; }

        /// <inheritdoc/>
        public override IQueryable<Site> ToQueryable(IQueryable<Site> query)
        {
            return query
                .WhereIf(Code.IsNotNullOrEmpty(), x => EF.Functions.Like(x.Code, $"%{Code}%"))
                .WhereIf(Name.IsNotNullOrEmpty(), x => EF.Functions.Like(x.Name, $"%{Name}%"));
        }
    }
}
