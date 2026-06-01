namespace PersonalHub.Infrastructure
{
    internal static class InfrastructureConstants
    {
        internal static class DatabaseProviders
        {
            public const string SqlServer = "Microsoft.EntityFrameworkCore.SqlServer";
            public const string Postgres = "Npgsql.EntityFrameworkCore.PostgreSQL";
        }
        internal static class SqlServerColumnTypes
        {
            public const string NVARCHAR = "nvarchar";
            public const string NCHAR = "nchar";
            public const string VARCHAR = "varchar";
            public const string CHAR = "char";
            public const string SMALLINT = "smallint";
            public const string INT = "int";
            public const string MONEY = "money";
            public const string DECIMAL = "decimal";
            public const string DATE = "date";
            public const string DATETIME = "datetime";
            public const string UNIQUEIDENTIFIER = "uniqueidentifier";
            public const string BIT = "bit";
        }
    }
}
