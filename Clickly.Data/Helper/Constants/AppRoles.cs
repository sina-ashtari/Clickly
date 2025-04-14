namespace Clickly.Data.Helper.Constants
{
    public static class AppRoles
    {
        public const string Admin = "Admin";
        public const string User = "User";
        public static readonly IReadOnlyList<string> All = new List<string>() { Admin, User};
        
    }
}
