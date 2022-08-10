using TripleSix.Core.Entities;
using TripleSix.Core.Types;

namespace TripleSix.Core.Services
{
    public interface ICreatableWithModel<TEntity, TModel> : IService<TEntity>
        where TEntity : class, IEntity
        where TModel : class, IDto
    {
        /// <summary>
        /// Khởi tạo entity với dữ liệu cung cấp.
        /// </summary>
        /// <typeparam name="TResult">Loại dữ liệu đầu ra.</typeparam>
        /// <param name="input">Dữ liệu đầu vào</param>
        /// <param name="generateCode">Có phát sinh code hay không.</param>
        /// <param name="cancellationToken">Token để cancel task.</param>
        /// <returns>Dữ liệu được ghi nhận.</returns>
        Task<TResult> CreateWithModel<TResult>(TModel input, bool generateCode, CancellationToken cancellationToken = default)
            where TResult : class;
    }
}
