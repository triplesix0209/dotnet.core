namespace Sample.Infrastructure.DataContext
{
    public partial class DataDbContext
    {
        /// <inheritdoc/>
        public DbSet<Test> Test { get; set; }
    }
}
