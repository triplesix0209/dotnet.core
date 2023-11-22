using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Elastic.Clients.Elasticsearch;

namespace TripleSix.Core.Elastic
{
    /// <summary>
    /// Elastic document.
    /// </summary>
    /// <typeparam name="TDocument">Document type.</typeparam>
    public abstract class BaseElasticDocument<TDocument> : IElasticDocument
        where TDocument : class, IElasticDocument
    {
        /// <inheritdoc/>
        public virtual string GetIndexName()
        {
            var elasticDocumentAttr = GetType().GetCustomAttribute<ElasticDocumentAttribute>() ?? throw new NullReferenceException();
            return elasticDocumentAttr.FullIndexName();
        }

        /// <inheritdoc/>
        public virtual string GetId()
        {
            var keyProperty = GetType().GetProperties().FirstOrDefault(x => x.GetCustomAttribute<KeyAttribute>() != null);
            var id = keyProperty?.GetValue(this)?.ToString();
            return id ?? throw new NullReferenceException();
        }

        /// <inheritdoc/>
        public virtual async Task<IndexResponse> Index(ElasticsearchClient client)
        {
            var data = this as TDocument;
            return await client.IndexAsync(data!, GetIndexName());
        }

        /// <inheritdoc/>
        public virtual async Task<DeleteResponse?> Delete(ElasticsearchClient client)
        {
            return await client.DeleteAsync(GetIndexName(), GetId());
        }
    }
}
