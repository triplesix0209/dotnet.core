﻿namespace Sample.Domain.Dto
{
    public class SettingAdminDto : AdminModel<Setting>
    {
        public class Filter : BaseAdminFilterDto<Setting>
        {
            [DisplayName("Lọc theo mã số")]
            public FilterParameterString Code { get; set; }
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
