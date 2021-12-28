using System;

namespace TripleSix.Core.Entities
{
    public interface IMapEntity : IEntity
    {
        DateTime? CreateDatetime { get; set; }

        Guid? CreatorId { get; set; }

        void SetIds(params Guid[] listIds);
    }
}