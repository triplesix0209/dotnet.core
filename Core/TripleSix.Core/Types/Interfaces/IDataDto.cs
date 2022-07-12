namespace TripleSix.Core.Types
{
    /// <summary>
    /// DTO dữ liệu.
    /// </summary>
    public interface IDataDto : IDto
    {
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
