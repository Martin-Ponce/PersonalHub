using Asp.Versioning;
using System.Reflection;

namespace PersonalHub.API.Helpers
{
    public class ApiConstants
    {
        private static readonly ApiVersion _version1 = new(1, 0);
        public static ApiVersion VERSION_1 => _version1;

        public static class Headers
        {
            public const string API_KEY = "X-Api-Key";
        }
        public static class Claims
        {
            public const string USER_ID = "userid";
        }
        public static class SupportedProviders
        {
            public enum SupportedProvidersName
            {
                SqlServer
            }
        }

        public static class Cookies
        {
            public const string ACCESS_TOKEN = "accessToken";
            public const string REFRESH_TOKEN = "refreshToken";
            public const string PERSISTENT_SESSION = "persistentSession";
        }
        public const string APP_NAME = "PersonalHub";
        public static string LOGGER_TABLE_NAME => $"{APP_NAME}Logs";
        public const string DATABASE_CONTEXT_CONNECTION_STRING_NAME = "PersonalHub";
        public const string EXPIRED_SESSIONS_CLEANER = "ExpiredSessionsCleaner";
        public const string ALLOW_ALL_CORS_POLICY = "AllowAll";
        public const string FIXED_RATE_LIMITING_POLICY = "Fixed";
        public const int MAX_PAGE_SIZE = 100;
        public const int MIN_PAGE_SIZE = 1;
        public static string ASSEMBLY_VERSION => Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? "";
    }
}
