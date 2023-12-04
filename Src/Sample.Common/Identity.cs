namespace Sample.Common
{
    public class Identity : BaseIdentity
    {
        public Identity()
            : base()
        {
        }

        public Identity(HttpContext httpContext)
            : base(httpContext)
        {
        }
    }
}
