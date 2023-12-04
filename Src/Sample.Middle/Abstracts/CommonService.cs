using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sample.Middle.Services;
using TripleSix.CoreOld.AutoAdmin;
using TripleSix.CoreOld.Dto;
using TripleSix.CoreOld.Entities;
using TripleSix.CoreOld.Repositories;

namespace Sample.Middle.Abstracts
{
    public abstract class CommonService<TEntity> : BaseCommonService<TEntity>
        where TEntity : class, IModelEntity
    {
        public CommonService(IModelRepository<TEntity> repo)
           : base(repo)
        {
        }

        public IAccountService AccountService { get; set; }

        protected override async Task<IEnumerable<ActorDto>> GetActor(IIdentity identity, params Guid[] actorIds)
        {
            return await AccountService.GetListById<ActorDto>(identity, actorIds, true);
        }
    }
}
