using TripleSix.Core.Persistences;

namespace Sample.Domain.Dto
{
    public class SettingFilterDto : BaseQueryDto<Setting>
    {
        [DisplayName("lọc theo mã số")]
        public string? Code { get; set; }

        public override IQueryable<Setting> ToQueryable(IDbDataContext db)
        {
            return db.Set<Setting>()
                .WhereIf(!Code.IsNullOrWhiteSpace(), x => EF.Functions.Like(x.Code!, $"%{Code}%"));
        }
    }
}
