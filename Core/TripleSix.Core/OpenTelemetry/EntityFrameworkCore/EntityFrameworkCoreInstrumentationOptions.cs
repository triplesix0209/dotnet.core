using TripleSix.Core.OpenTelemetry.Shared;

namespace TripleSix.Core.OpenTelemetry
{
    /// <summary>
    /// Tùy chỉnh cho <see cref="EntityFrameworkCoreInstrumentation"/>.
    /// </summary>
    public class EntityFrameworkCoreInstrumentationOptions
    {
        /// <summary>
        /// Ghi nhận tên của các Stored Procedure vào tag <see cref="SemanticConventions.AttributeDbStatement"/>. Mặc định là True.
        /// </summary>
        public bool SetDbStatementForStoredProcedure { get; set; } = true;

        /// <summary>
        /// Ghi nhận câu truy vấn SQL vào tag <see cref="SemanticConventions.AttributeDbStatement"/>. Mặc định là True.
        /// </summary>
        public bool SetDbStatementForText { get; set; } = true;

        /// <summary>
        /// Ghi nhận tham số SQL vào tag "db.parameters". Mặc định là True.
        /// </summary>
        public bool SetDbParameter { get; set; } = true;
    }
}
