namespace CpTech.Core.WebApi.Results
{
    public interface IError
    {
        string Code { get; set; }

        string Message { get; set; }
    }
}