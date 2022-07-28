namespace Sample.Infrastructure.Seeds
{
    public class PermissionDataSeed : BaseDataSeed
    {
        internal static readonly Guid DefaultPermissionGroupId = Guid.Parse("41097C99-A6C7-4056-9EF5-BE1DE1FDFE77");

        public override void OnDataSeeding(ModelBuilder builder)
        {
            var data = new[]
            {
                new
                {
                    CategoryCode = "profile",
                    CategoryName = "thông tin cá nhân",
                    Permissions = new[] { "update" },
                },
                new
                {
                    CategoryCode = "account",
                    CategoryName = "tài khoản",
                    Permissions = new[] { "create", "read", "update", "delete", "changelog", "export" },
                },
                new
                {
                    CategoryCode = "permission",
                    CategoryName = "quyền",
                    Permissions = new[] { "create", "read", "update", "delete", "changelog", "export" },
                },
                new
                {
                    CategoryCode = "setting",
                    CategoryName = "thiết lập",
                    Permissions = new[] { "read", "update", "changelog", "export" },
                },
            };

            var permissionGroup = new PermissionGroup()
            {
                Id = DefaultPermissionGroupId,
                Code = "Admin",
                Name = "Nhóm quyền quản trị",
            };
            builder.Entity<PermissionGroup>().HasData(permissionGroup);

            var permissions = new List<Permission>();
            var permissionValues = new List<PermissionValue>();
            foreach (var item in data)
            {
                foreach (var permissionItem in item.Permissions)
                {
                    var permission = new Permission
                    {
                        Code = item.CategoryCode + "." + permissionItem,
                        CategoryName = item.CategoryName.ToTitleCase(),
                        Name = permissionItem switch
                        {
                            "create" => $"Tạo {item.CategoryName}",
                            "read" => $"Đọc {item.CategoryName}",
                            "update" => $"Sửa {item.CategoryName}",
                            "delete" => $"Xóa {item.CategoryName}",
                            "changelog" => $"Xem log {item.CategoryName}",
                            "export" => $"Xuất {item.CategoryName}",
                            _ => item.CategoryName,
                        },
                    };
                    permissions.Add(permission);

                    permissionValues.Add(new PermissionValue
                    {
                        Code = permission.Code,
                        GroupId = permissionGroup.Id,
                        Value = PermissionValues.Allow,
                        ActualValue = true,
                    });
                }
            }

            builder.Entity<Permission>().HasData(permissions);
            builder.Entity<PermissionValue>().HasData(permissionValues);
        }
    }
}
