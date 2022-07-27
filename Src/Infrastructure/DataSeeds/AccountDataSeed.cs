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
            };

            var accounts = data.Select(x => new Account
            {
                Id = x.Id,
                Name = x.Name,
                AccessLevel = x.AccessLevel,
                PermissionGroupId = x.PermissionGroupId,
                IsDeleted = x.IsDeleted,
            });
            builder.Entity<Account>().HasData(accounts);

            var auths = data.Select(x => new AccountAuth
            {
                Id = x.Id,
                Username = x.Username,
                HashPasswordKey = x.HashPasswordKey,
                HashedPassword = AccountService.HashPassword(x.Password, x.HashPasswordKey),
                AccountId = x.Id,
            });
            builder.Entity<AccountAuth>().HasData(auths);
        }
    }
}
