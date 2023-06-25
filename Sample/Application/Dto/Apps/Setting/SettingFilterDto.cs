namespace Sample.Application.Dto.Apps
{
    public class SettingFilterDto : BaseQueryDto<Setting>
    {
        public string? Code { get; set; }

        public override IQueryable<Setting> ToQueryable(IQueryable<Setting> query)
        {
            return query
                .WhereNotDeleted()
                .WhereIf(!Code.IsNullOrWhiteSpace(), x => EF.Functions.Like(x.Code!, $"%{Code}%"));
        }
    }
}
