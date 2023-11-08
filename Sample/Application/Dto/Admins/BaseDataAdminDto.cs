namespace Sample.Application.Dto.Admins
{
    public abstract class BaseDataAdminDto : BaseDto
    {
        [DisplayName("Id định danh")]
        public Guid Id { get; set; }

        [DisplayName("Thời gian khởi tạo")]
        public DateTime? CreateAt { get; set; }

        [DisplayName("Thời gian chỉnh sửa cuối")]
        public DateTime? UpdateAt { get; set; }

        [DisplayName("Thời gian xóa")]
        public DateTime? DeleteAt { get; set; }
    }
}
