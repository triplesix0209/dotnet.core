namespace TripleSix.Core.Elastic
{
    /// <summary>
    /// Cấu hình elastic document.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class ElasticDocumentAttribute : Attribute
    {
        /// <summary>
        /// Cấu hình elastic document.
        /// </summary>
        /// <param name="indexName">Tên index.</param>
        /// <param name="templateName">Tên template.</param>
        public ElasticDocumentAttribute(string indexName, string? templateName = null)
        {
            IndexName = indexName;
            TemplateName = templateName;
        }

        /// <summary>
        /// Tên index.
        /// </summary>
        public string IndexName { get; }

        /// <summary>
        /// Tên template.
        /// </summary>
        public string? TemplateName { get; }
    }
}
