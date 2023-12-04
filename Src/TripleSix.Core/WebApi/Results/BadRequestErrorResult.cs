using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using TripleSix.Core.Helpers;

namespace TripleSix.Core.WebApi.Results
{
    public class BadRequestErrorResult : ErrorResult<Dictionary<string, List<string>>>
    {
        public BadRequestErrorResult(ModelStateDictionary modelStateDictionary)
            : base(400, "bad_client_request", "thông tin đầu vào bị thiếu hoặc không đúng")
        {
            Error.Data = new Dictionary<string, List<string>>();

            var errorItems = modelStateDictionary
                .Where(item => item.Value.ValidationState == ModelValidationState.Invalid);

            foreach (var (key, value) in errorItems)
            {
                Error.Data.Add(
                    key.ToCamelCase(),
                    value.Errors.AsEnumerable()
                        .Select(x => x.ErrorMessage)
                        .ToList());
            }
        }
    }
}