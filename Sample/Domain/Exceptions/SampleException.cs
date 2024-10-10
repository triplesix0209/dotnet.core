namespace Sample.Domain.Exceptions
{
    public class SampleException : BaseException
    {
        public SampleException()
            : base("Sample exception")
        {
        }

        public override int HttpCodeStatus => 500;
    }
}
