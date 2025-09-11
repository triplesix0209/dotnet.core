using Microsoft.AspNetCore.Http;

namespace Sample.Application.Dto.Admins
{
    public class AccountCreateAdminDto : BaseInputDto<Account>
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public Guid SiteId { get; set; }

        public override async Task<Account> MapToEntity(IMapper mapper, IServiceProvider serviceProvider)
        {
            var result = await base.MapToEntity(mapper, serviceProvider);
            result.Code = RandomHelper.RandomString(10);
            return result;
        }

        public override void OnValidate(ref FluentValidation.Results.ValidationResult validationResult, HttpContext? httpContext)
        {
            validationResult.Errors.Add(new FluentValidation.Results.ValidationFailure("a", "b"));
        }
    }
}
