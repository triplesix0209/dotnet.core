namespace CpTech.Core.Dto
{
    public interface IPaging<TItem>
    {
        int Page { get; set; }

        int Size { get; set; }

        long Total { get; set; }

        TItem[] Items { get; set; }
    }
}