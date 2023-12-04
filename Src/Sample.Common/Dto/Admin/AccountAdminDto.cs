using System;
using System.ComponentModel;
using Sample.Common.Enum;

namespace Sample.Common.Dto
{
    public class AccountAdminDto : BaseAdminDto
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
            [DisplayName("tên gọi")]
            public string Name { get; set; }

            [DisplayName("ảnh đại diện")]
            [AdminField(Type = AdminFieldTypes.Media, GroupName = "hình ảnh")]
            public string AvatarLink { get; set; }

            [DisplayName("e-mail")]
            public string Email { get; set; }

            [DisplayName("username")]
            [AdminField(GroupName = "đăng nhập")]
            public string Username { get; set; }

            [DisplayName("cấp độ tài khoản")]
            [AdminField(GroupName = "phân quyền")]
            public AccountLevels AccessLevel { get; set; }

            [DisplayName("mã nhóm quyền")]
            [AdminField(ModelType = typeof(PermissionGroupAdminDto), DisplayBy = nameof(PermissionGroupName), GroupName = "phân quyền")]
            public Guid? PermissionGroupId { get; set; }

            [DisplayName("tên nhóm quyền")]
            [AdminField(GroupName = "phân quyền")]
            public string PermissionGroupName { get; set; }
        }

        public class Detail : Item
        {
            [DisplayName("đã xác thực e-mail?")]
            public bool? IsEmailVerified { get; set; }
        }

        public class Create : DataDto
        {
            [DisplayName("tên gọi")]
            [StringLengthValidate(100)]
            [RequiredValidate]
            public string Name { get; set; }

            [DisplayName("e-mail")]
            [StringLengthValidate(100)]
            public string Email { get; set; }

            [DisplayName("đã xác thực e-mail?")]
            public bool? IsEmailVerified { get; set; }

            [DisplayName("ảnh đại diện")]
            [AdminField(Type = AdminFieldTypes.Media, GroupName = "hình ảnh")]
            public string AvatarLink { get; set; }

            [DisplayName("username")]
            [AdminField(GroupName = "đăng nhập")]
            [StringLengthValidate(100)]
            [RequiredValidate]
            public string Username { get; set; }

            [DisplayName("mật khẩu")]
            [AdminField(GroupName = "đăng nhập")]
            [RequiredValidate]
            public string Password { get; set; }

            [DisplayName("cấp độ tài khoản")]
            [AdminField(GroupName = "phân quyền")]
            [RequiredValidate]
            [EnumValidate]
            public AccountLevels? AccessLevel { get; set; }

            [DisplayName("mã nhóm quyền")]
            [AdminField(ModelType = typeof(PermissionGroupAdminDto), ScriptDisplay = "data.accessLevel !== '1'", GroupName = "phân quyền")]
            public Guid? PermissionGroupId { get; set; }
        }

        public class Update : DataDto
        {
            [DisplayName("tên gọi")]
            [StringLengthValidate(100)]
            [RequiredValidate]
            public string Name { get; set; }

            [DisplayName("e-mail")]
            [StringLengthValidate(100)]
            public string Email { get; set; }

            [DisplayName("đã xác thực e-mail?")]
            public bool? IsEmailVerified { get; set; }

            [DisplayName("ảnh đại diện")]
            [AdminField(Type = AdminFieldTypes.Media, GroupName = "hình ảnh")]
            public string AvatarLink { get; set; }

            [DisplayName("username")]
            [AdminField(GroupName = "đăng nhập")]
            [StringLengthValidate(100)]
            [RequiredValidate]
            public string Username { get; set; }

            [DisplayName("mật khẩu")]
            [AdminField(GroupName = "đăng nhập")]
            public string Password { get; set; }

            [DisplayName("cấp độ tài khoản")]
            [AdminField(GroupName = "phân quyền")]
            [RequiredValidate]
            [EnumValidate]
            public AccountLevels? AccessLevel { get; set; }

            [DisplayName("mã nhóm quyền")]
            [AdminField(ModelType = typeof(PermissionGroupAdminDto), ScriptDisplay = "data.accessLevel !== '1'", GroupName = "phân quyền")]
            public Guid? PermissionGroupId { get; set; }
        }
    }
}
