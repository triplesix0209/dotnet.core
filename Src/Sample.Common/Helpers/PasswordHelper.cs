namespace Sample.Common.Helpers
{
    public static class PasswordHelper
    {
        public static string Hash(string hashKey, string password)
        {
            return HashHelper.MD5Hash(password + hashKey);
        }

        public static bool Check(string hashKey, string password, string hashedPassword)
        {
            return HashHelper.MD5Hash(password + hashKey) == hashedPassword;
        }
    }
}
