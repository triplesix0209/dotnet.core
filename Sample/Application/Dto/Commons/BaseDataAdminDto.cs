namespace Sample.Application.Dto.Commons
{
    public abstract class BaseDataAdminDto : BaseDto
    {
        public Guid Id { get; set; }

        public DateTime? CreateAt { get; set; }

        public DateTime? UpdateAt { get; set; }

        public DateTime? DeleteAt { get; set; }
    }
}
