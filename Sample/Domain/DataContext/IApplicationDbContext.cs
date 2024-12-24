namespace Sample.Domain.DataContext
{
    public interface IApplicationDbContext : IDbDataContext, IDbMigrationContext
    {
        DbSet<Account> Account { get; set; }

        DbSet<Site> Site { get; set; }
    }
}
