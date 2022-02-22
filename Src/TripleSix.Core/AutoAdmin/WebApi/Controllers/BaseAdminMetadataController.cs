using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using TripleSix.Core.WebApi.Controllers;

namespace TripleSix.Core.AutoAdmin
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public abstract class BaseAdminMetadataController : BaseController
    {
        private static readonly AdminMetadata _metadata = new AdminMetadata();

        public static void Validate()
        {
            foreach (var controller in _metadata.Controllers)
            {
                foreach (var method in controller.Methods)
                {
                    if (controller.Methods.Count(x => x.Type == method.Type) > 1)
                        throw new Exception($"Admin Controller \"{controller.Name}\": has \"{method.Type}\" method is duplicated");

                    switch (method.Type)
                    {
                        case AdminMethodTypes.List:
                            if ((method as MethodListMetadata).ItemFields.Count(x => x.IsModelKey) != 1)
                                throw new Exception($"List Controller \"{controller.Name}\": has invalid model key field");
                            if ((method as MethodListMetadata).ItemFields.Count(x => x.IsModelText) != 1)
                                throw new Exception($"List Controller \"{controller.Name}\": has invalid model text field");
                            break;

                        case AdminMethodTypes.Detail:
                            if ((method as MethodDetailMetadata).DetailFields.Count(x => x.IsModelKey) != 1)
                                throw new Exception($"Detail Controller \"{controller.Name}\": has invalid model key field");
                            if ((method as MethodDetailMetadata).DetailFields.Count(x => x.IsModelText) != 1)
                                throw new Exception($"Detail Controller \"{controller.Name}\": has invalid model text field");
                            break;
                    }
                }
            }
        }

        [HttpGet]
        public virtual IActionResult Metadata()
        {
            return DataResult(_metadata);
        }
    }
}
