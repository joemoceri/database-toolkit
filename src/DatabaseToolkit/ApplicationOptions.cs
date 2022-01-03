namespace DatabaseToolkit
{
    internal class ApplicationOptions
    {
        public string ApplicationName { get; set; }
        public string Version { get; set; }
        public IDictionary<string, string> ConnectionStrings { get; set; }

        public string GetSqlServerConnectionString
        {
            get
            {
                return Environment.GetEnvironmentVariable("SqlServerDatabase") ?? ConnectionStrings["SqlServerDatabase"];
            }
        }

        public string GetMySqlConnectionString
        {
            get
            {
                return Environment.GetEnvironmentVariable("MySqlDatabase") ?? ConnectionStrings["MySqlDatabase"];
            }
        }

        public string SqlServerBasePath { get; set; }

        public string MySqlDumpPath { get; set; }

        public string MySqlDefaultsFilePath { get; set; }

        public string PostgreSQLHost { get; set; }

        public string PostgreSQLPort { get; set; }

        public string PostgreSQLUser { get; set; }

    }
}
