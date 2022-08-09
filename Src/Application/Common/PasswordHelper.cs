namespace Sample.Application.Common
{
    public static class PasswordHelper
    {
        public static string HashPassword(string password, string hashKey)
        {
            return HashHelper.MD5Hash(password + hashKey);
        }
    }
}
