namespace Sample.Application.Dto.Admins
{
    public class AccountFilterAdminDto : BaseQueryDto<Account>
    {
        public string? Code { get; set; }

        public string? Name { get; set; }

        public override IQueryable<Account> ToQueryable(IQueryable<Account> query)
        {
            return query
                .WhereIf(!Code.IsNullOrWhiteSpace(), x => x.Code == Code)
                .WhereIf(!Name.IsNullOrWhiteSpace(), x => EF.Functions.Like(x.Name!, $"%{Name}%"));
        }
    }
}
