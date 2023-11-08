namespace TripleSix.Core.WebApi
{
    /// <summary>
    /// Controller endpoint.
    /// </summary>
    /// <typeparam name="TController">Kiểu controler.</typeparam>
    public interface IControllerEndpoint<TController>
        where TController : BaseController
    {
    }
}
