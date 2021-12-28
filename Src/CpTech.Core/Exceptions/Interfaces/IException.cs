namespace CpTech.Core.Exceptions
{
    public interface IException
    {
        int HttpCode { get; }

        string Code { get; }

        string Message { get; }
    }
}