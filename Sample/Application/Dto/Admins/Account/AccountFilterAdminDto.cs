namespace Sample.Application.Dto.Admins
{
    public class AccountFilterAdminDto : BaseFilterAdminDto<Account>
    {
        public string? Test { get; set; }

        public string? Code { get; set; }

        public string? Name { get; set; }

        public Guid? SiteId { get; set; }

        /// <inheritdoc/>
        public override IQueryable<Account> ToQueryable(IQueryable<Account> query)
        {
            return query
                .WhereIf(Code.IsNotNullOrEmpty(), x => EF.Functions.Like(x.Code, $"%{Code}%"))
                .WhereIf(Name.IsNotNullOrEmpty(), x => EF.Functions.Like(x.Name, $"%{Name}%"))
                .WhereIf(SiteId.HasValue, x => x.SiteId == SiteId);
        }
    }
}
