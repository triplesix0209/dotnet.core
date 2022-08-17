namespace Sample.Domain.Dto
{
    public class SettingAdminDto : AdminModel<Setting>
    {
        public class Filter : BaseAdminFilterDto<Setting>
        {
            [DisplayName("Lọc theo mã số")]
            public FilterParameterString Code { get; set; }

            [DisplayName("Lọc theo giá trị")]
            public FilterParameterString Value { get; set; }

            [DisplayName("Lọc theo mô tả")]
            public FilterParameterString Description { get; set; }
        }

        public class Item : BaseAdminItemDto
        {
            [DisplayName("Mã số")]
            public string Code { get; set; }

            [DisplayName("giá trị")]
            public string? Value { get; set; }

            [DisplayName("mô tả")]
            public string? Description { get; set; }
        }

        public class Detail : Item
        {
        }

        public class Create : BaseDto
        {
            [DisplayName("Mã số")]
            [Required]
            public string Code { get; set; }

            [DisplayName("giá trị")]
            [Required(AllowEmptyStrings = true)]
            public string? Value { get; set; }

            [DisplayName("mô tả")]
            [NotEmpty]
            public string? Description { get; set; }
        }
    }
}
