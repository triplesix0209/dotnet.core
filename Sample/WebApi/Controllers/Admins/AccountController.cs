namespace Sample.WebApi.Controllers.Admins
{
    [SwaggerTag("Tài khoản")]
    [AdminReadEndpoint<AccountController, Account, AccountDataAdminDto, AccountDetailAdminDto, AccountFilterAdminDto>]
    [AdminCreateEndpoint<AccountController, Account, AccountCreateAdminDto>]
    [AdminUpdateEndpoint<AccountController, Account, AccountUpdateAdminDto>]
    [AdminSoftDeleteEndpoint<AccountController, Account>]
    public class AccountController : AdminController
    {
    }
}
