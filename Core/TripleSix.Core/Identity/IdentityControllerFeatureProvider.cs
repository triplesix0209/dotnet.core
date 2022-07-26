using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace TripleSix.Core.Identity
{
    public class IdentityControllerFeatureProvider
        : IApplicationFeatureProvider<ControllerFeature>
    {
        /// <inheritdoc/>
        public void PopulateFeature(IEnumerable<ApplicationPart> parts, ControllerFeature feature)
        {
        }
    }
}
