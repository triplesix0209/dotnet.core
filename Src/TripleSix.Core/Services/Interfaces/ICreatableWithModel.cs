using System.Collections.Generic;
using System.Threading.Tasks;
using TripleSix.Core.Dto;

namespace TripleSix.Core.Services
{
    public interface ICreatableWithModel<TModel>
        where TModel : DataDto
    {
        Task<TResult> CreateWithModel<TResult>(IIdentity identity, TModel input, bool autoGenerateCode = true)
            where TResult : class;

        async Task<IEnumerable<TResult>> CreateBulkWithModel<TResult>(IIdentity identity, IEnumerable<TModel> input, bool autoGenerateCode = true)
            where TResult : class
        {
            var result = new List<TResult>();
            foreach (var inputItem in input)
                result.Add(await CreateWithModel<TResult>(identity, inputItem, autoGenerateCode));

            return result;
        }
    }
}
