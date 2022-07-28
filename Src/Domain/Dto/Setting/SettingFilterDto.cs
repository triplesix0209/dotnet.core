namespace Sample.Domain.Dto
{
    public class SettingFilterDto : BaseQueryDto<Setting>
    {
        [DisplayName("lọc theo mã số")]
        public string? Code { get; set; }

        public override IQueryable<Setting> ToQueryable(IApplicationDbContext db)
        {
            return db.Setting
                .WhereIf(!Code.IsNullOrWhiteSpace(), x => EF.Functions.Like(x.Code!, $"%{Code}%"));
        }
    }
}
