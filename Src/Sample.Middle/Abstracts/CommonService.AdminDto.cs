using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TripleSix.Core.AutoAdmin;
using TripleSix.Core.Dto;
using TripleSix.Core.Entities;
using TripleSix.Core.Repositories;

namespace Sample.Middle.Abstracts
{
    public abstract class CommonService<TEntity, TAdminDto> : BaseCommonService<TEntity, TAdminDto>
        where TEntity : class, IModelEntity
        where TAdminDto : BaseAdminDto
    {
        public CommonService(IModelRepository<TEntity> repo)
           : base(repo)
        {
        }

        protected override Task<IEnumerable<ActorDto>> GetActor(IIdentity identity, params Guid[] actorIds)
        {
            return null;
        }
    }
}
