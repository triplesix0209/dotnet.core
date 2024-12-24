namespace Sample.Domain.DataContext
{
    public interface IDataDbContext : IDbDataContext, IDbMigrationContext
    {
        DbSet<Test> Test { get; set; }
    }
}
