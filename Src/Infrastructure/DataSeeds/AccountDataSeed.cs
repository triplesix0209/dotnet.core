using Sample.Application.Services;

namespace Sample.Infrastructure.Seeds
{
    public class AccountDataSeed : BaseDataSeed
    {
        public override void OnDataSeeding(ModelBuilder builder)
        {
            var data = new[]
            {
                new
                {
                    SiteId = Guid.Parse("5de826db-9f83-428a-9c26-06d53a7fefb1"),
                    SiteName = "Điện Máy Chợ Lớn",
                    Accounts = new[]
                    {
                        new
                        {
                            Id = Guid.Parse("653dc4d4-ca05-45ac-83cd-e98fa91b890f"),
                            Name = "Root",
                            Username = "root",
                            Password = "root",
                            HashPasswordKey = "8sBXJjPl1BaK1ppd0PNMB366NHhmAx",
                            AccessLevel = AccountLevels.Root,
                            PermissionGroupId = (Guid?)null,
                            IsDeleted = true,
                        },
                        new
                        {
                            Id = Guid.Parse("B81D0C90-3B91-44D4-BB00-95A5925FA5C6"),
                            Name = "Admin",
                            Username = "admin",
                            Password = "admin",
                            HashPasswordKey = "xE8czZlAixQOJDQ0oR7PqlYJUcywj6",
                            AccessLevel = AccountLevels.Admin,
                            PermissionGroupId = (Guid?)PermissionDataSeed.DefaultPermissionGroupId,
                            IsDeleted = false,
                        },
                    },
                },
            };

            var accounts = new List<Account>();
            var auths = new List<AccountAuth>();

            foreach (var site in data)
            {
                foreach (var account in site.Accounts)
                {
                    accounts.Add(new Account
                    {
                        Id = account.Id,
                        Name = account.Name,
                        AccessLevel = account.AccessLevel,
                        PermissionGroupId = account.PermissionGroupId,
                        IsDeleted = account.IsDeleted,
                    });

                    auths.Add(new AccountAuth
                    {
                        Id = account.Id,
                        Username = account.Username,
                        HashPasswordKey = account.HashPasswordKey,
                        HashedPassword = AccountService.HashPasswordWithKey(account.Password, account.HashPasswordKey),
                        AccountId = account.Id,
                    });
                }
            }

            builder.Entity<Account>().HasData(accounts);
            builder.Entity<AccountAuth>().HasData(auths);
        }
    }
}
