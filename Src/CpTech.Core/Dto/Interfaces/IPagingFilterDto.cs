namespace CpTech.Core.Dto
{
    public interface IPagingFilterDto
        : IFilterDto
    {
        int Page { get; set; }

        int Size { get; set; }
    }
}