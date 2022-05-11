using System.Linq;
using Sample.Common.Dto;
using Sample.Common.Enum;
using Sample.Data.Entities;
using TripleSix.Core.Mappers;

namespace Sample.Middle.Mappers
{
    public class IdentityMapper : BaseMapper
    {
        public IdentityMapper()
        {
            CreateMapToEntity<IdentityUpdateProfileDto, AccountEntity>(AutoMapper.MemberList.Source);

            CreateMapFromEntity<AccountEntity, IdentityProfileDto>()
                .ForMember(d => d.AccountId, o => o.MapFrom(s => s.Id))
                .ForMember(d => d.Username, o => o.MapFrom(s => s.Auths.FirstOrDefault(x => x.Type == AccountAuthTypes.UsernamePassword).Username));
        }
    }
}
