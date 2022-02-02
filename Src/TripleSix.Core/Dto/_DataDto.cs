#pragma warning disable SA1649 // File name should match first type name

using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace TripleSix.Core.Dto
{
    public abstract class DataDto : BaseDto,
        IDataDto
    {
        private HashSet<string> _propertyTracking = new HashSet<string>();
        private HttpContext _httpContext = null;

        public override object Clone()
        {
            var result = (DataDto)base.Clone();
            result._propertyTracking = _propertyTracking;
            return result;
        }

        public void ClearPropertyTracking()
        {
            _propertyTracking.Clear();
        }

        public virtual bool IsAnyPropertyChanged()
        {
            return _propertyTracking.Any();
        }

        public virtual bool IsPropertyChanged(string propertyName)
        {
            return _propertyTracking.Any(x => x == propertyName);
        }

        public virtual void SetPropertyChanged(string propertyName, bool value)
        {
            if (value)
            {
                _propertyTracking.Add(propertyName);
            }
            else
            {
                _propertyTracking.RemoveWhere(x => x == propertyName);
            }
        }

        public void SetHttpContext(HttpContext httpContext)
        {
            _httpContext = httpContext;
        }

        public HttpContext GetHttpContext()
        {
            return _httpContext;
        }
    }
}
