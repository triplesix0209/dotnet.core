using System.Reflection;
using Microsoft.Extensions.Configuration;
using TripleSix.Core.DataContexts;

namespace Sample.Data.DataContexts
{
    public partial class DataContext : MySqlContext
    {
        public DataContext(IConfiguration configuration)
            : base(Assembly.GetExecutingAssembly(), configuration)
        {
        }
    }
}
