using Elastic.Clients.Elasticsearch;

namespace TripleSix.Core.Elastic
{
    /// <summary>
    /// Elastic document.
    /// </summary>
    public interface IElasticDocument
    {
        /// <summary>
        /// Lấy tên index.
        /// </summary>
        /// <returns>Tên index.</returns>
        string GetIndexName();

        /// <summary>
        /// Lấy id (property được đánh dấu bởi [Key]).
        /// </summary>
        /// <returns>Giá trị id.</returns>
        string GetId();

        /// <summary>
        /// Tạo hoặc cập nhật dữ liệu với elastic.
        /// </summary>
        /// <param name="client"><see cref="ElasticsearchClient"/> sử dụng để kết nối.</param>
        /// <returns><see cref="IndexResponse"/>.</returns>
        Task<IndexResponse> Index(ElasticsearchClient client);

        /// <summary>
        /// Xóa dữ liệu với elastic.
        /// </summary>
        /// <param name="client"><see cref="ElasticsearchClient"/> sử dụng để kết nối.</param>
        /// <returns><see cref="DeleteResponse"/>.</returns>
        Task<DeleteResponse?> Delete(ElasticsearchClient client);
    }
}
