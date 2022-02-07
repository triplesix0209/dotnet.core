using Microsoft.EntityFrameworkCore;

namespace Sample.Data.DataContexts
{
    public partial class DataContext
    {
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
