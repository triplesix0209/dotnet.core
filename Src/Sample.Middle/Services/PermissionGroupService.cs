using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sample.Common.Dto;
using Sample.Common.Enum;
using Sample.Data.Entities;
using Sample.Data.Repositories;
using Sample.Middle.Abstracts;
using TripleSix.Core.Dto;
using TripleSix.Core.Helpers;
using TripleSix.Core.Mappers;

namespace Sample.Middle.Services
{
    public class PermissionGroupService : CommonService<PermissionGroupEntity, PermissionGroupAdminDto>,
        IPermissionGroupService
    {
        public PermissionGroupService(PermissionGroupRepository repo)
            : base(repo)
        {
        }

        public PermissionRepository PermissionRepo { get; set; }

        public PermissionGroupRepository PermissionGroupRepo { get; set; }

        public PermissionValueRepository PermissionGroupValueRepo { get; set; }

        public async Task<PermissionValueDto[]> GetListPermissionValue(IIdentity identity, Guid? id = null)
        {
            var result = new List<PermissionValueDto>();

            PermissionGroupEntity group = null;
            if (id.HasValue) group = await GetFirstById(identity, id.Value);

            var listPermission = await PermissionRepo.Query.ToArrayAsync<PermissionEntity>(Mapper);
            foreach (var permission in listPermission)
            {
                var resultItem = new PermissionValueDto
                {
                    Code = permission.Code,
                    Name = permission.Name,
                    CategoryName = permission.CategoryName,
                };

                if (group == null)
                {
                    resultItem.Value = PermissionValues.Inherit;
                    resultItem.ActualValue = false;
                    result.Add(resultItem);
                    continue;
                }

                var valueItem = await GetPermissionValue(identity, group.Id, permission.Code);
                if (valueItem != null)
                {
                    resultItem.Value = valueItem.Value;
                    resultItem.ActualValue = valueItem.ActualValue;
                    result.Add(resultItem);
                    continue;
                }

                var parentGroup = group.HierarchyParent;
                PermissionValueEntity parentPermissionValue = null;
                if (parentGroup != null) parentPermissionValue = await GetPermissionValue(identity, parentGroup.Id, permission.Code);
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

            return result.ToArray();
        }

        public async Task<PermissionGroupAdminDto.Detail> ConvertEntityToModel(IIdentity identity, PermissionGroupEntity entity, PermissionGroupAdminDto.Detail model = null)
        {
            var result = Mapper.MapData(entity, model);
            result.ListPermissionValue = await GetListPermissionValue(identity, result.Id);
            return result;
        }

        public async Task<TResult> CreateWithModel<TResult>(IIdentity identity, PermissionGroupAdminDto.Create input, bool autoGenerateCode = true)
            where TResult : class
        {
            var entity = await CreateWithMapper<PermissionGroupEntity>(identity, input, autoGenerateCode);

            PermissionGroupEntity parent = null;
            if (entity.HierarchyParentId.HasValue)
                parent = await GetFirstById(identity, entity.HierarchyParentId.Value);

            foreach (var item in input.ListPermissionValue)
            {
                var permission = await GetPermission(identity, item.Code);
                var valueItem = new PermissionValueEntity
                {
                    PermissionGroupId = entity.Id,
                    PermissionCode = permission.Code,
                    Value = item.Value,
                    ActualValue = item.Value == PermissionValues.Allow,
                };

                if (valueItem.Value == PermissionValues.Inherit && parent != null)
                {
                    var parentValueItem = await GetPermissionValue(identity, parent.Id, permission.Code);
                    valueItem.ActualValue = parentValueItem == null ? false : parentValueItem.ActualValue;
                }

                PermissionGroupValueRepo.Create(valueItem);
            }

            await SaveChanges();
            return Mapper.Map<TResult>(entity);
        }

        public async Task UpdateWithModel(IIdentity identity, Guid id, PermissionGroupAdminDto.Update input)
        {
            await UpdateWithMapper(identity, id, input);

            var group = await GetFirstById(identity, id);
            var parent = group.HierarchyParent;

            foreach (var item in input.ListPermissionValue)
            {
                var permission = await GetPermission(identity, item.Code);
                var permissionValue = await GetPermissionValue(identity, group.Id, permission.Code);

                if (permissionValue == null)
                {
                    permissionValue = new PermissionValueEntity
                    {
                        PermissionGroupId = group.Id,
                        PermissionCode = permission.Code,
                        Value = item.Value,
                        ActualValue = item.Value == PermissionValues.Allow,
                    };

                    if (permissionValue.Value == PermissionValues.Inherit && parent != null)
                    {
                        var parentValue = await GetPermissionValue(identity, parent.Id, permission.Code);
                        permissionValue.ActualValue = parentValue == null ? false : parentValue.ActualValue;
                    }

                    PermissionGroupValueRepo.Create(permissionValue);
                }
                else
                {
                    permissionValue.Value = item.Value;
                    permissionValue.ActualValue = item.Value == PermissionValues.Allow;
                    if (permissionValue.Value == PermissionValues.Inherit && parent != null)
                    {
                        var parentValue = await GetPermissionValue(identity, parent.Id, permission.Code);
                        permissionValue.ActualValue = parentValue == null ? false : parentValue.ActualValue;
                    }

                    PermissionGroupValueRepo.Update(permissionValue);
                }
            }

            await SyncPermissionValue(identity, group.Id, group: group);
            await SaveChanges();
        }

        private async Task<PermissionEntity> GetPermission(IIdentity identity, string code)
        {
            return await PermissionRepo.Query
                .Where(x => x.Code == code)
                .FirstAsync<PermissionEntity>(Mapper);
        }

        private async Task<PermissionValueEntity> GetPermissionValue(IIdentity identity, Guid groupId, string permissionCode)
        {
            return await PermissionGroupValueRepo.Query
                   .Where(x => x.PermissionGroupId == groupId && x.PermissionCode == permissionCode)
                   .FirstOrDefaultAsync<PermissionValueEntity>(Mapper);
        }

        private async Task SyncPermissionValue(
            IIdentity identity, Guid groupId, bool saveChange = false, PermissionGroupEntity group = null, PermissionEntity[] listPermission = null)
        {
            if (group == null)
                group = await GetFirstById(identity, groupId);
            if (listPermission == null)
                listPermission = await PermissionRepo.Query.ToArrayAsync<PermissionEntity>(Mapper);

            foreach (var permission in listPermission)
            {
                var permissionValue = await GetPermissionValue(identity, group.Id, permission.Code);
                if (permissionValue != null && permissionValue.Value != PermissionValues.Inherit)
                    continue;

                var isCreate = false;
                if (permissionValue == null)
                {
                    isCreate = true;
                    permissionValue = new PermissionValueEntity
                    {
                        PermissionGroupId = group.Id,
                        PermissionCode = permission.Code,
                        Value = PermissionValues.Inherit,
                        ActualValue = false,
                    };
                }

                if (group.HierarchyParentId.HasValue)
                {
                    var parentValue = await GetPermissionValue(identity, group.HierarchyParentId.Value, permission.Code);
                    permissionValue.ActualValue = parentValue.ActualValue;
                }

                if (isCreate) PermissionGroupValueRepo.Create(permissionValue);
                else PermissionGroupValueRepo.Update(permissionValue);
            }

            var childGroups = await GetList(identity, PermissionGroupRepo.Query
                .Where(x => x.HierarchyParentId == groupId));
            foreach (var childGroup in childGroups)
                await SyncPermissionValue(identity, childGroup.Id, saveChange, childGroup, listPermission);
        }
    }
}
