using System;

namespace TripleSix.Core.Dto
{
    public interface IModelFilterDto
        : IPagingFilterDto
    {
        FilterParameterString Search { get; set; }

        FilterParameter<Guid> Id { get; set; }

        FilterParameterString Code { get; set; }

        FilterParameter<bool> IsDeleted { get; set; }

        FilterParameterDatetime CreateDatetime { get; set; }

        FilterParameterDatetime UpdateDatetime { get; set; }

        FilterParameter<Guid> CreatorId { get; set; }

        FilterParameter<Guid> UpdaterId { get; set; }
    }
}