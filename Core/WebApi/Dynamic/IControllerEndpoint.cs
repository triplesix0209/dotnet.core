namespace TripleSix.Core.WebApi
{
    /// <summary>
    /// Controller endpoint.
    /// </summary>
    /// <typeparam name="TController">Kiểu controler.</typeparam>
    /// <typeparam name="TEndpointAttribute">Endpoint attribute.</typeparam>
    public interface IControllerEndpoint<TController, TEndpointAttribute>
        where TController : BaseController
        where TEndpointAttribute : IControllerEndpointAttribute
    {
    }
}
