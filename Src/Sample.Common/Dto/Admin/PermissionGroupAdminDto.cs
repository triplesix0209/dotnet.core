using System;
using System.ComponentModel;
using TripleSix.Core.Attributes;
using TripleSix.Core.AutoAdmin;
using TripleSix.Core.Dto;

namespace Sample.Common.Dto
{
    public class PermissionGroupAdminDto : BaseAdminDto
    {
        public class Filter : ModelFilterDto
        {
            [DisplayName("lọc theo nhóm cha")]
            [AdminField(Type = AdminFieldTypes.HierarchyParentId)]
            public FilterParameter<Guid> HierarchyParentId { get; set; }

            [DisplayName("lọc theo tên gọi")]
            public FilterParameter<string> Name { get; set; }
        }

        public class Item : ModelDataDto
        {
            [DisplayName("tên gọi")]
            [AdminField(IsModelText = true)]
            public string Name { get; set; }

            [DisplayName("mã nhóm cha")]
            [AdminField(Type = AdminFieldTypes.HierarchyParentId, DisplayBy = nameof(HierarchyParentName))]
            public Guid? HierarchyParentId { get; set; }

            [DisplayName("tên nhóm cha")]
            public string HierarchyParentName { get; set; }
        }

        public class Detail : Item
        {
            [DisplayName("danh sách quyền")]
            [AdminField(GridCol = 12)]
            public PermissionValueDto[] ListPermissionValue { get; set; }
        }

        public class Create : DataDto
        {
            [DisplayName("tên gọi")]
            [StringLengthValidate(100)]
            [RequiredValidate]
            public string Name { get; set; }

            [DisplayName("nhóm cha")]
            [AdminField(Type = AdminFieldTypes.HierarchyParentId)]
            public Guid? HierarchyParentId { get; set; }

            [DisplayName("danh sách quyền")]
            [AdminField(GridCol = 12)]
            public PermissionValueDto[] ListPermissionValue { get; set; }
        }

        public class Update : DataDto
        {
            [DisplayName("mã số")]
            [StringLengthValidate(32)]
            public string Code { get; set; }

            [DisplayName("tên gọi")]
            [StringLengthValidate(100)]
            [RequiredValidate]
            public string Name { get; set; }

            [DisplayName("nhóm cha")]
            [AdminField(Type = AdminFieldTypes.HierarchyParentId)]
            public Guid? HierarchyParentId { get; set; }

            [DisplayName("danh sách quyền")]
            [AdminField(GridCol = 12)]
            public PermissionValueDto[] ListPermissionValue { get; set; }
        }

        public class ListPermissionValue : DataDto
        {
            [DisplayName("mã nhóm quyền")]
            public Guid? Id { get; set; }
        }
    }
}
