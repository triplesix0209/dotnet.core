namespace Sample.Domain.DataContext
{
    public interface IDataDbContext
    //: IDbDataContext
    {
        DbSet<Test> Test { get; set; }
    }
}
