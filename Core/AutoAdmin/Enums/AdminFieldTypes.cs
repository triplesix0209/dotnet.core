namespace TripleSix.Core.AutoAdmin
{
    /// <summary>
    /// Phân loại field của admin.
    /// </summary>
    public enum AdminFieldTypes
    {
        /// <summary>
        /// Khung text nhiều dòng.
        /// </summary>
        TextArea = 1,

        /// <summary>
        /// Hình ảnh / Video.
        /// </summary>
        Media = 2,

        /// <summary>
        /// Khung chỉnh sửa HTML.
        /// </summary>
        HTMLEditor = 3,

        /// <summary>
        /// Mã định danh mục cha.
        /// </summary>
        HierarchyParentId = 4,
    }
}
