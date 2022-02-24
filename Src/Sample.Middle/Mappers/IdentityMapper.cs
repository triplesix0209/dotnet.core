using Sample.Common.Dto;
using Sample.Data.Entities;
using TripleSix.Core.Mappers;

namespace Sample.Middle.Mappers
{
    public class IdentityMapper : BaseMapper
    {
        public IdentityMapper()
        {
            CreateMapToEntity<IdentityUpdateProfileDto, AccountEntity>(AutoMapper.MemberList.Source);
        }
    }
}
