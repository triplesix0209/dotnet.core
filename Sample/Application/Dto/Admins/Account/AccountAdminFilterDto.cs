namespace Sample.Application.Dto.Admins
{
    public class AccountAdminFilterDto : BaseQueryDto<Account>
    {
        public string? Code { get; set; }

        public string? Name { get; set; }

        public Guid? SiteId { get; set; }

        public override IQueryable<Account> ToQueryable(IQueryable<Account> query)
        {
            return query
                .WhereIf(!Code.IsNullOrEmpty(), x => EF.Functions.Like(x.Code!, $"%{Code}%"))
                .WhereIf(!Name.IsNullOrEmpty(), x => EF.Functions.Like(x.Name!, $"%{Name}%"))
                .WhereIf(SiteId.HasValue, x => x.SiteId == SiteId);
        }
    }
}
