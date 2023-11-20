﻿namespace Sample.Application.Dto.Admins
{
    [MapToEntity(typeof(Account))]
    [IgnoreUnmapProperty]
    public class AccountUpdateAdminDto : BaseDto
    {
        public string Code { get; set; }

        public string Name { get; set; }

        public Guid SiteId { get; set; }
    }
}
