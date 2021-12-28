namespace TripleSix.Core.WebApi.Results
{
    public class ErrorResult : BaseResult<BaseMeta>
    {
        public ErrorResult(
            int statusCode = 500,
            string code = "exception",
            string message = "internal server error")
            : base(statusCode)
        {
            Meta = new BaseMeta { Success = false };
            Error = new BaseError { Code = code, Message = message };
        }

        public virtual BaseError Error { get; protected set; }
    }
}