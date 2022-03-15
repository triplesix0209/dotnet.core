using System;
using System.ComponentModel;
using Sample.Common.Enum;
using TripleSix.Core.Attributes;
using TripleSix.Core.AutoAdmin;
using TripleSix.Core.Dto;

namespace Sample.Common.Dto
{

    public class SoldierAdminDto : BaseAdminDto
    {
        public class Filter : ModelFilterDto
        {
            [DisplayName("lọc theo tên gọi")]
            public FilterParameterString Name { get; set; }

            [DisplayName("lọc theo e-mail")]
            public FilterParameterString Email { get; set; }

            [DisplayName("lọc theo username")]
            public FilterParameterString Username { get; set; }

            [DisplayName("lọc theo nhóm quyền")]
            [AdminField(ModelType = typeof(PermissionGroupAdminDto))]
            public FilterParameter<Guid> PermissionGroupId { get; set; }

            [DisplayName("lọc theo cấp độ tài khoản")]
            public FilterParameter<AccountLevels> AccessLevel { get; set; }
        }

        public class Item : ModelDataDto
        {
            [DisplayName("ảnh đại diện")]
            [AdminField(Type = AdminFieldTypes.Media, GroupName = "hình ảnh")]
            public string AvatarLink { get; set; }

            [DisplayName("e-mail")]
            public string Email { get; set; }

            [DisplayName("tên gọi")]
            public string Name { get; set; }

            [DisplayName("username")]
            public string Username { get; set; }

            [DisplayName("cấp độ tài khoản")]
            public AccountLevels AccessLevel { get; set; }

            [DisplayName("mã nhóm quyền")]
            [AdminField(ModelType = typeof(PermissionGroupAdminDto), DisplayBy = nameof(PermissionGroupName))]
            public Guid? PermissionGroupId { get; set; }

            [DisplayName("tên nhóm quyền")]
            public string PermissionGroupName { get; set; }
        }

        public class Detail : Item
        {
            [DisplayName("đã xác thực e-mail?")]
            public bool? IsEmailVerified { get; set; }
        }

        public class Create : DataDto
        {
            [DisplayName("mã số")]
            [Description("bỏ trống để tự phát sinh")]
            [StringLengthValidate(100)]
            public string Code { get; set; }

            [DisplayName("e-mail")]
            [StringLengthValidate(100)]
            public string Email { get; set; }

            [DisplayName("tên gọi")]
            [StringLengthValidate(100)]
            [RequiredValidate]
            public string Name { get; set; }

            [DisplayName("username")]
            [StringLengthValidate(100)]
            [RequiredValidate]
            public string Username { get; set; }

            [DisplayName("mật khẩu")]
            [RequiredValidate]
            public string Password { get; set; }

            [DisplayName("ảnh đại diện")]
            [AdminField(Type = AdminFieldTypes.Media, GroupName = "hình ảnh")]
            public string AvatarLink { get; set; }

            [DisplayName("cấp độ tài khoản")]
            [RequiredValidate]
            [EnumValidate]
            public AccountLevels? AccessLevel { get; set; }

            [DisplayName("mã nhóm quyền")]
            [AdminField(ModelType = typeof(PermissionGroupAdminDto), ScriptDisplay = "data.accessLevel !== '1'")]
            public Guid? PermissionGroupId { get; set; }

            [DisplayName("đã xác thực e-mail?")]
            public bool? IsEmailVerified { get; set; }
        }

        public class Update : DataDto
        {
            [DisplayName("mã số")]
            [StringLengthValidate(100)]
            public string Code { get; set; }

            [DisplayName("e-mail")]
            [StringLengthValidate(100)]
            public string Email { get; set; }

            [DisplayName("tên gọi")]
            [StringLengthValidate(100)]
            [RequiredValidate]
            public string Name { get; set; }

            [DisplayName("username")]
            [StringLengthValidate(100)]
            [RequiredValidate]
            public string Username { get; set; }

            [DisplayName("mật khẩu")]
            public string Password { get; set; }

            [DisplayName("ảnh đại diện")]
            [AdminField(Type = AdminFieldTypes.Media, GroupName = "hình ảnh")]
            public string AvatarLink { get; set; }

            [DisplayName("cấp độ tài khoản")]
            [RequiredValidate]
            [EnumValidate]
            public AccountLevels? AccessLevel { get; set; }

            [DisplayName("mã nhóm quyền")]
            [AdminField(ModelType = typeof(PermissionGroupAdminDto), ScriptDisplay = "data.accessLevel !== '1'")]
            public Guid? PermissionGroupId { get; set; }

            [DisplayName("đã xác thực e-mail?")]
            public bool? IsEmailVerified { get; set; }
        }
    }
}
