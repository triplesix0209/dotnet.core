namespace TripleSix.Core.WebApi.Results
{
    public class ErrorResult<TData> : BaseResult<BaseMeta>
    {
        public ErrorResult(
            int statusCode = 500,
            string code = "exception",
            string message = "internal server error",
            TData data = default)
            : base(statusCode)
        {
            Meta = new BaseMeta { Success = false };
            Error = new DataError<TData> { Code = code, Message = message, Data = data };
        }

        public virtual DataError<TData> Error { get; protected set; }
    }
}