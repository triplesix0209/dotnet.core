using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using FluentValidation;
using TripleSix.Core.Helpers;

namespace Sample.WebApi.Controllers.Admins
{
    public class TestController : CommonController
    {
        [HttpPut]
        [Transactional]
        public async Task<SuccessResult> Test([FromBody] AccountAuthAdminSetDto input)
        {
            return SuccessResult();
        }

        public class AccountAuthAdminSetDto : BaseDto
        {
            [Required]
            public AccountAuthFieldTypes Field { get; set; }

            [Required]
            public string Value { get; set; }

            public bool IsPrimary { get; set; }

            public bool IsVerified { get; set; }
        }

        public class AccountAuthAdminUpdateValidator : BaseValidator<AccountAuthAdminSetDto>
        {
            public AccountAuthAdminUpdateValidator()
                : base()
            {
                RuleFor(x => x.Value)
                    .NotEmpty()
                    .Must(value => value.IsNullOrEmpty() || !value.Contains(' ')).WithMessage("'{PropertyName}' cannot not include space")
                    .When(x => x.Field == AccountAuthFieldTypes.Username);

                RuleFor(x => x.Value)
                    .NotEmpty()
                    .Must(value => value.IsNullOrEmpty() || value == value.Trim()).WithMessage("'{PropertyName}' must be trim.")
                    .When(x => x.Field == AccountAuthFieldTypes.Email);

                RuleFor(x => x.Value)
                    .NotEmpty()
                    .Must(value => value.IsNullOrEmpty() || Regex.IsMatch(value, @"^[0-9]+$")).WithMessage("'{PropertyName}' must be number.")
                    .When(x => x.Field == AccountAuthFieldTypes.PhoneNumber);
            }
        }

        public enum AccountAuthFieldTypes
        {
            [Description("Tên đăng nhập")]
            Username = 1,

            [Description("Số điện thoại")]
            PhoneNumber = 2,

            [Description("E-mail")]
            Email = 3,
        }
    }
}
