namespace Sample.Domain.Dto
{
    public class SettingAdminDto : AdminModel<Setting>
    {
        public class Filter : BaseAdminFilterDto<Setting>
        {
        }

        public class Item : BaseAdminItemDto
        {
            [DisplayName("Mã số")]
            public string Code { get; set; }

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
