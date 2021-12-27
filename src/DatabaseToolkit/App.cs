using Microsoft.Extensions.Options;

namespace DatabaseToolkit
{
    internal class App
    {
        private readonly IOptions<ApplicationOptions> options;
        private readonly ISQLServerToolkit sqlServerToolkit;
        private readonly IMySQLToolkit mySqlToolkit;

        public App(IOptions<ApplicationOptions> options, ISQLServerToolkit sqlServerToolkit, IMySQLToolkit mySqlToolkit)
        {
            this.options = options;
            this.sqlServerToolkit = sqlServerToolkit;
            this.mySqlToolkit = mySqlToolkit;
        }

        public void Run()
        {
            var databaseName = "YourDatabaseName";
            var localDatabasePath = "C:\\backups\\YourDatabaseBackup.bak";

            // there are two ways you can call this. If you don't specify localDatabasePath, it will generate it for you using the appsettings.json SQLServerBasePath value.
            //sqlServerToolkit.RestoreDatabase(databaseName);
            //sqlServerToolkit.RestoreDatabase(databaseName, localDatabasePath);

            //sqlServerToolkit.BackupDatabase(databaseName);
            //sqlServerToolkit.BackupDatabase(databaseName, localDatabasePath);
        }
    }
}
