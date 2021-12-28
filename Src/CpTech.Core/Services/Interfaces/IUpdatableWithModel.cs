using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CpTech.Core.Dto;

namespace CpTech.Core.Services
{
    public interface IUpdatableWithModel<TModel>
        where TModel : DataDto
    {
        Task UpdateWithModel(IIdentity identity, Guid id, TModel input);

        async Task UpdateBulkWithModel(IIdentity identity, IEnumerable<Guid> listId, TModel input)
        {
            foreach (var id in listId)
                await UpdateWithModel(identity, id, input);
        }
    }
}
