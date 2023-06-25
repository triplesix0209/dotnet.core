namespace Sample.Application.Dto.Apps
{
    [MapFromEntity(typeof(Account))]
    public class AccountDataDto : BaseDto
    {
        public Guid Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }
    }
}
