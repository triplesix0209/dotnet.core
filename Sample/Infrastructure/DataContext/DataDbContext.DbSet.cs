namespace Sample.Infrastructure.DataContext
{
    public partial class DataDbContext
    //: IDataDbContext
    {
        /// <inheritdoc/>
        public DbSet<Test> Test { get; set; }
    }
}
