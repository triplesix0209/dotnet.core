using System.ComponentModel;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using TripleSix.Core.Entities;

namespace TripleSix.Core.Types
{
    /// <summary>
    /// DTO cơ bản.
    /// </summary>
    public interface IDto : INotifyPropertyChanged
    {
        /// <summary>
        /// Kiểm tra dữ liệu với các <see cref="IValidator"/>.
        /// </summary>
        /// <param name="validator">Các <see cref="IValidator"/> dùng để kiểm tra dữ liệu. Bỏ trống hệ thống sẽ tự tìm validator phù hợp.</param>
        /// <param name="httpContext"><see cref="HttpContext"/>.</param>
        /// <param name="throwOnFailures">Throw exception khi có lỗi.</param>
        /// <returns><see cref="ValidationResult"/>.</returns>
        ValidationResult Validate(IValidator? validator = default, HttpContext? httpContext = default, bool throwOnFailures = false);

        /// <summary>
        /// Chuẩn hóa dữ liệu.
        /// </summary>
        void Normalize();

        /// <summary>
        /// Thao tác sau khi lấy dữ liệu đơn.
        /// </summary>
        /// <param name="entity">Entity.</param>
        /// <param name="serviceProvider">Service provider.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task AfterGetFirst(IEntity entity, IServiceProvider serviceProvider);

        /// <summary>
        /// Kiểm tra có bất kỳ property nào được thay đổi.
        /// </summary>
        /// <returns><c>True</c> nếu có ít nhất một property bị thay đổi, ngược lại là <c>False</c>.</returns>
        bool IsAnyPropertyChanged();

        /// <summary>
        /// Kiểm tra property chỉ định có thay đổi hay không.
        /// </summary>
        /// <param name="name">Tên property cần kiểm tra.</param>
        /// <returns><c>True</c> nếu property bị thay đổi, ngược lại là <c>False</c>.</returns>
        bool IsPropertyChanged(string name);

        /// <summary>
        /// Đánh dấu property chỉ định có sự thay đổi.
        /// </summary>
        /// <param name="name">Tên property cần đánh dấu.</param>
        /// <param name="value"><c>True</c> nếu property bị thay đổi, ngược lại là <c>False</c>.</param>
        void SetPropertyChanged(string name, bool value);
    }
}
