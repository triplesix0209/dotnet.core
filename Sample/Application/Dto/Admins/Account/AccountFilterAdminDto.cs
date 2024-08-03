namespace Sample.Application.Dto.Admins
{
    public class AccountFilterAdminDto : BaseFilterAdminDto<Account>
    {
        public string? Test { get; set; }

        public string? Code { get; set; }

        public string? Name { get; set; }

        public Guid? SiteId { get; set; }

        public override Task<IQueryable<Account>> ToQueryable(IQueryable<Account> query, IServiceProvider serviceProvider)
        {
            var result = query
                .WhereIf(Code.IsNotNullOrEmpty(), x => EF.Functions.Like(x.Code, $"%{Code}%"))
                .WhereIf(Name.IsNotNullOrEmpty(), x => EF.Functions.Like(x.Name, $"%{Name}%"))
                .WhereIf(SiteId.HasValue, x => x.SiteId == SiteId);

            return Task.FromResult(result);
        }
    }
}
