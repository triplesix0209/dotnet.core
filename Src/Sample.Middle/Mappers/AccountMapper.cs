using System.Linq;
using Sample.Common.Dto;
using Sample.Common.Enum;
using Sample.Data.Entities;
using TripleSix.Core.AutoAdmin;
using TripleSix.Core.Mappers;

namespace Sample.Middle.Mappers
{
    public class AccountMapper : BaseMapper
    {
        public AccountMapper()
        {
            CreateMapFromEntity<AccountEntity, ActorDto>();

            CreateMap<AccountEntity, AccountAdminDto.Item>()
                .ForMember(d => d.Username, o => o.MapFrom(s => s.Auths.FirstOrDefault(x => x.Type == AccountAuthTypes.UsernamePassword).Username));

            CreateMap<AccountEntity, AccountAdminDto.Detail>()
                .IncludeBase<AccountEntity, AccountAdminDto.Item>();
        }
    }
}
