using System.Linq.Expressions;
using System.Security.Claims;

namespace TripleSix.Core.Identity
{
    public interface IIdentityContext
    {
        /// <summary>
        /// Khởi tạo danh sách claim.
        /// </summary>
        /// <typeparam name="T">Loại dữ liệu đầu vào.</typeparam>
        /// <param name="data">Dữ liệu đầu vào, mỗi property sẽ được chuyển thành các claim.</param>
        /// <param name="accessLevel">Cấp độ tài khoản.</param>
        /// <param name="idSelector">Hàm lấy Id từ data, nếu bỏ trống sẽ thử lấy property Id.</param>
        /// <param name="permissions">Danh sách quyền.</param>
        /// <param name="customGenerator">Hàm phát sinh tùy chỉnh.</param>
        /// <returns>Danh sách <see cref="Claim"/>.</returns>
        List<Claim> GenerateClaim<T>(T data, int accessLevel, Expression<Func<T, Guid>>? idSelector = default, IEnumerable<string>? permissions = default, Func<T, List<Claim>>? customGenerator = default);
    }
}
