namespace Sample.Application.Services
{
    public class PermissionService : StrongService<PermissionGroup>, IPermissionService
    {
        public Task<List<PermissionItemDto>> GetAllPermission()
        {
            return Db.Permission.ToListAsync<PermissionItemDto>(Mapper);
        }

        public async Task<List<PermissionValueDto>> GetListPermissionValue(Guid id, bool grantedOnly)
        {
            var result = new List<PermissionValueDto>();

            var group = await GetById(id, false);

            var allPermissions = await GetAllPermission();
            foreach (var permission in allPermissions)
            {
                var resultItem = new PermissionValueDto
                {
                    Code = permission.Code,
                    Name = permission.Name,
                    CategoryName = permission.CategoryName,
                };

                var valueItem = group.PermissionValues.FirstOrDefault(x => x.Code == group.Code);
                if (valueItem != null)
                {
                    resultItem.Value = valueItem.Value;
                    resultItem.ActualValue = valueItem.ActualValue;
                    result.Add(resultItem);
                    continue;
                }

                var parentGroup = group.Parent;
                PermissionValue? parentPermissionValue = null;
                if (parentGroup != null) parentPermissionValue = await GetPermissionValue(parentGroup.Id, permission.Code);
                if (parentGroup == null || parentPermissionValue == null)
                {
                    resultItem.Value = PermissionValues.Inherit;
                    resultItem.ActualValue = false;
                    result.Add(resultItem);
                    continue;
                }

                resultItem.Value = PermissionValues.Inherit;
                resultItem.ActualValue = parentPermissionValue.ActualValue;
                result.Add(resultItem);
            }

            return result;
        }

        protected async Task<PermissionValue?> GetPermissionValue(Guid groupId, string permissionCode)
        {
            return await Db.PermissionValue
                   .Where(x => x.GroupId == groupId && x.Code == permissionCode)
                   .FirstOrDefaultAsync();
        }
    }
}
