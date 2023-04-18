namespace Sample.Application.Services
{
    public class ObjectLogService : BaseObjectLogService
    {
        public IApplicationDbContext Db { get; set; }

        protected override async Task<ActorDto> GetActor(Guid actorId, CancellationToken cancellationToken = default)
        {
            var account = await Db.Account
                .Where(x => x.Id == actorId)
                .FirstOrDefaultAsync(cancellationToken);
            if (account == null)
                return new ActorDto { Id = actorId };

            return new ActorDto
            {
                Id = account.Id,
                Name = account.Name,
                AvatarLink = null,
            };
        }
    }
}
