using Microsoft.AspNetCore.Mvc.Controllers;

namespace TripleSix.Core.Types
{
    /// <summary>
    /// Biến đổi scope.
    /// </summary>
    public interface IScopeTransformer
    {
        /// <summary>
        /// Chuyển đổi scope.
        /// </summary>
        /// <param name="inputScope">Scope đầu vào.</param>
        /// <param name="controller">Thông tin controller.</param>
        /// <returns>Scope sau khi biến đổi.</returns>
        string Transform(string inputScope, ControllerActionDescriptor controller);
    }
}
