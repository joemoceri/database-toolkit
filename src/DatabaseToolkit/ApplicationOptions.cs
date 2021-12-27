namespace DatabaseToolkit
{
    internal class ApplicationOptions
    {
        public string ApplicationName { get; set; }
        public string Version { get; set; }
        public IDictionary<string, string> ConnectionStrings { get; set; }

        public string GetConnectionString
        {
            get
            {
                return Environment.GetEnvironmentVariable("Database") ?? ConnectionStrings["Database"];
            }
        }

        public string SqlServerBasePath { get; set; }

    }
}
