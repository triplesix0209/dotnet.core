namespace Sample.Application.Dto.Admins
{
    public class SiteAdminFilterDto : BaseQueryDto<Site>
    {
        public string? Code { get; set; }

        public string? Name { get; set; }

        public override IQueryable<Site> ToQueryable(IQueryable<Site> query)
        {
            return query
                .WhereIf(!Code.IsNullOrEmpty(), x => EF.Functions.Like(x.Code!, $"%{Code}%"))
                .WhereIf(!Name.IsNullOrEmpty(), x => EF.Functions.Like(x.Name!, $"%{Name}%"));
        }
    }
}
