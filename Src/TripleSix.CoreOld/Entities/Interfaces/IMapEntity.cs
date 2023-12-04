using System;

namespace TripleSix.CoreOld.Entities
{
    public interface IMapEntity : IEntity
    {
        DateTime? CreateDatetime { get; set; }

        Guid? CreatorId { get; set; }
    }
}