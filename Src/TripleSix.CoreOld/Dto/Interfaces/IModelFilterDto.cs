using System;

namespace TripleSix.CoreOld.Dto
{
    public interface IModelFilterDto
        : IPagingFilterDto
    {
        FilterParameter<Guid> Id { get; set; }

        FilterParameterString Code { get; set; }

        FilterParameter<bool> IsDeleted { get; set; }

        FilterParameterDatetime CreateDatetime { get; set; }

        FilterParameterDatetime UpdateDatetime { get; set; }

        FilterParameter<Guid> CreatorId { get; set; }

        FilterParameter<Guid> UpdaterId { get; set; }

        string Search { get; set; }

        SortColumn[] SortColumn { get; set; }
    }
}
