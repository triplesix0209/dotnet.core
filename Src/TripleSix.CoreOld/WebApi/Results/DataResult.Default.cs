namespace TripleSix.CoreOld.WebApi.Results
{
    public class DataResult<TData> : DataResult<TData, SuccessMeta>
    {
        public DataResult(TData data = default)
            : base(data)
        {
        }
    }
}