namespace TripleSix.Core.WebApi.Results
{
    public class DataResult<TData> : DataResult<TData, BaseMeta>
    {
        public DataResult(TData data = default)
            : base(data)
        {
        }
    }
}