using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Elastic.Clients.Elasticsearch;

namespace TripleSix.Core.Elastic
{
    /// <summary>
    /// Elastic document.
    /// </summary>
    public abstract class BaseElasticDocument : IElasticDocument
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
            return await client.IndexAsync(this, GetIndexName());
        }

        /// <inheritdoc/>
        public virtual async Task<DeleteResponse?> Delete(ElasticsearchClient client)
        {
            return await client.DeleteAsync(GetIndexName(), GetId());
        }
    }
}
