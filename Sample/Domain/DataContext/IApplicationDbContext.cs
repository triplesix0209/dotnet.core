namespace Sample.Domain.DataContext
{
    public interface IApplicationDbContext : IDbDataContext
    {
        DbSet<Account> Account { get; set; }

        DbSet<Setting> Setting { get; set; }
    }
}
