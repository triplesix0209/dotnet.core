using System;

namespace TripleSix.Core.Dto
{
    public interface IModelFilterDto
        : IPagingFilterDto
    {
        string Search { get; set; }

        FilterParameter<Guid[]> Id { get; set; }

        //string AppendIds { get; set; }

        //Guid? NotId { get; set; }

        //string Code { get; set; }

        //string ListId { get; set; }

        //string NotListId { get; set; }

        //bool? IsDeleted { get; set; }

        //DateTime? StartCreateDatetime { get; set; }

        //DateTime? EndCreateDatetime { get; set; }

        //DateTime? StartUpdateDatetime { get; set; }

        //DateTime? EndUpdateDatetime { get; set; }

        //Guid? CreatorId { get; set; }

        //Guid? UpdaterId { get; set; }

        //string SortColumns { get; set; }
    }
}