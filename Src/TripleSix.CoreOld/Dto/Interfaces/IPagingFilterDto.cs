namespace TripleSix.CoreOld.Dto
{
    public interface IPagingFilterDto
        : IFilterDto
    {
        int Page { get; set; }

        int Size { get; set; }
    }
}