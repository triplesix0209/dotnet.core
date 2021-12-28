using System;

namespace CpTech.Core.Entities
{
    public interface IMapEntity : IEntity
    {
        DateTime? CreateDatetime { get; set; }

        Guid? CreatorId { get; set; }

        void SetIds(params Guid[] listIds);
    }
}