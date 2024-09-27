namespace Documancer.Application.Extensions
{
    public static class Constant
    {
        // Routes:

        public const string RegisterRoute = "api/account/identity/create";
        public const string LoginRoute = "api/account/identity/login";
        public const string RefreshTokenRoute = "api/account/identity/refresh-token";
        public const string GetRolesRoute = "api/account/identity/role/list";
        public const string CreateRoleRoute = "api/account/identity/role/create";
        public const string CreateAdminRoute = "setting";
        public const string GetUsersWithRolesRoute = "api/account/identity/users-with-roles";
        public const string ChangeUserRoleRoute = "api/account/identity/change-role";

        // Jwt Authentication:

        public const string AuthenticationType = "JwtAuth";

        // Browser Local Storage:

        public const string BrowserStorageKey = "x-key";

        // HttpClient:

        public const string HttpClientName = "DocumancerClient";
        public const string HttpClientHeaderScheme = "Bearer";

        // Account User Roles:

        public static class Role
        {
            public const string Admin = "Admin";
            public const string User = "User";
        }
    }
}
