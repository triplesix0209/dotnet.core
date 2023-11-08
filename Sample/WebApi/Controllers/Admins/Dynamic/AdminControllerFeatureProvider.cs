using System.Reflection;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;
using Sample.Domain.Entities;

namespace Sample.WebApi.Controllers.Admins
{
    public class AdminControllerFeatureProvider : IApplicationFeatureProvider<ControllerFeature>
    {
        public void PopulateFeature(IEnumerable<ApplicationPart> parts, ControllerFeature feature)
        {
            var readEndpoint = typeof(AdminReadEndpoint<,,>).MakeGenericType(typeof(Account), typeof(AccountDataAdminDto), typeof(AccountFilterAdminDto)).GetTypeInfo();
            feature.Controllers.Add(readEndpoint);
        }
    }
}
