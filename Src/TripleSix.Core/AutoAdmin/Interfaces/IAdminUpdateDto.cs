using TripleSix.Core.Dto;

namespace TripleSix.Core.AutoAdmin
{
    public interface IAdminUpdateDto : IDataDto
    {
        string ChangeLogNote { get; set; }
    }
}
