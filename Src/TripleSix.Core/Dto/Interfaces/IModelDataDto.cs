using System;

namespace TripleSix.Core.Dto
{
    public interface IModelDataDto
        : IDataDto
    {
        Guid? Id { get; set; }

        string Code { get; set; }

        bool? IsDeleted { get; set; }

        DateTime? CreateDatetime { get; set; }

        DateTime? UpdateDatetime { get; set; }

        Guid? CreatorId { get; set; }

        Guid? UpdaterId { get; set; }
    }
}