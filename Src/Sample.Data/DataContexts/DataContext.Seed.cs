using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Sample.Common.Enum;
using Sample.Common.Helpers;
using Sample.Data.Entities;

namespace Sample.Data.DataContexts
{
    public partial class DataContext
    {
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            var assembly = Assembly.GetExecutingAssembly();

            #region [setting]

            var settings = new List<(string Key, SettingEntity Data)>();
            using (var stream = assembly.GetManifestResourceStream("Sample.Data.Resources.Setting.json"))
            using (var reader = new StreamReader(stream))
            {
                var data = JArray.Parse(reader.ReadToEnd());
                foreach (var item in data)
                {
                    var setting = new SettingEntity
                    {
                        Id = Guid.Parse(item.Value<string>("id")),
                        Code = item.Value<string>("code"),
                        Description = item.Value<string>("description"),
                        Value = item.Value<string>("value"),
                    };
                    settings.Add((setting.Code, setting));
                }
            }

            if (settings.Any())
                builder.Entity<SettingEntity>().HasData(settings.Select(x => x.Data));

            #endregion

            #region [permission]

            var permissions = new List<(string Key, PermissionEntity Data)>();
            using (var stream = assembly.GetManifestResourceStream("Sample.Data.Resources.Permission.json"))
            using (var reader = new StreamReader(stream))
            {
                var data = JArray.Parse(reader.ReadToEnd());
                foreach (var category in data)
                {
                    var categoryCode = category["categoryCode"].Value<string>().ToCamelCase();
                    var categoryName = category["categoryName"].Value<string>().ToLower();

                    foreach (var item in category["permissions"])
                    {
                        var permission = new PermissionEntity();

                        if (item.Type == JTokenType.Object)
                        {
                            permission.Code = item.Value<string>("code");
                            permission.Name = item.Value<string>("name");
                        }
                        else
                        {
                            permission.Code = item.Value<string>().ToCamelCase();
                            switch (permission.Code)
                            {
                                case "create":
                                    permission.Name = "tạo {0}";
                                    break;
                                case "read":
                                    permission.Name = "xem {0}";
                                    break;
                                case "update":
                                    permission.Name = "sửa {0}";
                                    break;
                                case "delete":
                                    permission.Name = "xóa {0}";
                                    break;
                                case "changelog":
                                    permission.Name = "lịch sử thay đổi của {0}";
                                    break;
                                case "export":
                                    permission.Name = "xuất dữ liệu {0}";
                                    break;
                            }

                            if (categoryName.IsNotNullOrWhiteSpace())
                                permission.Name = string.Format(permission.Name, categoryName).Trim();
                        }

                        if (categoryCode.IsNotNullOrWhiteSpace())
                            permission.Code = categoryCode + "." + permission.Code;
                        if (categoryName.IsNotNullOrWhiteSpace())
                            permission.CategoryName = categoryName;

                        permissions.Add((permission.Code, permission));
                    }
                }
            }

            if (permissions.Any())
                builder.Entity<PermissionEntity>().HasData(permissions.Select(x => x.Data));

            #endregion

            #region [permission group]

            var permissionGroups = new List<(string Key, PermissionGroupEntity Data)>();
            var permissionGroupValues = new List<(string Key, PermissionValueEntity Data)>();
            using (var stream = assembly.GetManifestResourceStream("Sample.Data.Resources.PermissionGroup.json"))
            using (var reader = new StreamReader(stream))
            {
                var data = JArray.Parse(reader.ReadToEnd());
                foreach (var item in data)
                {
                    var permissionGroup = new PermissionGroupEntity
                    {
                        Id = Guid.Parse(item["id"].Value<string>()),
                        Code = item["code"].Value<string>(),
                        Name = item["name"].Value<string>(),
                    };
                    permissionGroups.Add((permissionGroup.Code, permissionGroup));

                    if (item["listPermission"].Value<string>() == "full")
                    {
                        foreach (var permission in permissions)
                        {
                            var permissionGroupValue = new PermissionValueEntity
                            {
                                PermissionGroupId = permissionGroup.Id,
                                PermissionCode = permission.Data.Code,
                                Value = PermissionValues.Allow,
                                ActualValue = true,
                            };

                            permissionGroupValues.Add((permissionGroup.Code + "." + permissionGroupValue.PermissionCode, permissionGroupValue));
                        }
                    }
                }
            }

            if (permissionGroups.Any())
                builder.Entity<PermissionGroupEntity>().HasData(permissionGroups.Select(x => x.Data));
            if (permissionGroupValues.Any())
                builder.Entity<PermissionValueEntity>().HasData(permissionGroupValues.Select(x => x.Data));

            #endregion

            #region [account]

            var accounts = new List<(string Key, AccountEntity Data)>();
            var auths = new List<(string Key, AccountAuthEntity Data)>();
            using (var stream = assembly.GetManifestResourceStream("Sample.Data.Resources.Account.json"))
            using (var reader = new StreamReader(stream))
            {
                var data = JArray.Parse(reader.ReadToEnd());
                foreach (var item in data)
                {
                    var account = new AccountEntity();
                    account.Id = Guid.Parse(item.Value<string>("id"));
                    account.Name = item.Value<string>("name");
                    account.Code = item.Value<string>("code").IsNotNullOrWhiteSpace()
                        ? account.Code = item.Value<string>("code")
                        : item.Value<string>("username");
                    account.AccessLevel = Enum.Parse<AccountLevels>(item.Value<string>("level"), true);
                    account.IsDeleted = item.Value<bool?>("isDeleted") == true;
                    account.PermissionGroupId = account.AccessLevel == AccountLevels.Root ? null : permissionGroups.First(x => x.Key == "admin").Data.Id;

                    var auth = new AccountAuthEntity();
                    auth.Id = account.Id;
                    auth.Type = AccountAuthTypes.UsernamePassword;
                    auth.Username = item.Value<string>("username");
                    auth.HashPasswordKey = item.Value<string>("hashPasswordKey");
                    auth.HashedPassword = PasswordHelper.Hash(auth.HashPasswordKey, item.Value<string>("password"));
                    auth.AccountId = account.Id;

                    accounts.Add((account.Code, account));
                    auths.Add((auth.Code, auth));
                }
            }

            if (accounts.Any())
                builder.Entity<AccountEntity>().HasData(accounts.Select(x => x.Data));
            if (auths.Any())
                builder.Entity<AccountAuthEntity>().HasData(auths.Select(x => x.Data));

            #endregion
        }
    }
}
