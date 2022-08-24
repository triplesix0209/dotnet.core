using System.ComponentModel;

namespace TripleSix.Core.AutoAdmin
{
    /// <summary>
    /// Thông tin log.
    /// </summary>
    public class RouteLog
    {
        [DisplayName("Id log")]
        public Guid LogId { get; set; }
    }
}
