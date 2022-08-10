namespace Sample.Domain.Dto
{
    [AdminModel(EntityType = typeof(Setting))]
    public class SettingAdminDto : BaseAdminDto
    {
        public class Filter : PagingInputDto, IQueryableDto<Setting>
        {
            public IQueryable<Setting> ToQueryable(IQueryable<Setting> query)
            {
                return query;
            }
        }

        public class Item : BaseDto
        {
            [DisplayName("giá trị")]
            public string Value { get; set; }

            [DisplayName("mô tả")]
            public string Description { get; set; }
        }

        public class Detail : Item
        {
        }
    }
}
