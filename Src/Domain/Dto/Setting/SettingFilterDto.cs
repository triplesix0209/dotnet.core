namespace Sample.Domain.Dto
{
    public class SettingFilterDto : BaseQueryDto<Setting>
    {
        [DisplayName("lọc theo mã số")]
        public string? Code { get; set; }

        public override IQueryable<Setting> ToQueryable(IQueryable<Setting> query)
        {
            return query
                .WhereIf(!Code.IsNullOrWhiteSpace(), x => EF.Functions.Like(x.Code!, $"%{Code}%"));
        }
    }
}
