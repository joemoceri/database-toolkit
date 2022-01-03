using Microsoft.Extensions.Options;

namespace DatabaseToolkit
{
    internal class App
    {
        private readonly IOptions<ApplicationOptions> options;
        private readonly ISQLServerToolkit sqlServerToolkit;
        private readonly IMySQLToolkit mySqlToolkit;
        private readonly IPostgreSQLToolkit postgreSqlToolkit;

        public App(IOptions<ApplicationOptions> options, ISQLServerToolkit sqlServerToolkit, IMySQLToolkit mySqlToolkit, IPostgreSQLToolkit postgreSqlToolkit)
        {
            this.options = options;
            this.sqlServerToolkit = sqlServerToolkit;
            this.mySqlToolkit = mySqlToolkit;
            this.postgreSqlToolkit = postgreSqlToolkit;
        }

        public void Run()
        {
            // sql server
            //var databaseName = "";
            //var localDatabasePath = "C:\\backups\\YourDatabaseBackup.bak";

            // sql server
            // there are two ways you can call this. If you don't specify localDatabasePath, it will generate it for you using the appsettings.json SQLServerBasePath value.
            //sqlServerToolkit.RestoreDatabase(databaseName);
            //sqlServerToolkit.RestoreDatabase(databaseName, localDatabasePath);
            //sqlServerToolkit.BackupDatabase(databaseName);
            //sqlServerToolkit.BackupDatabase(databaseName, localDatabasePath);

            // mysql
            //var databaseName = "sakila";
            //var localDatabasePath = "C:\\backups\\sakila.sql";

            // mysql
            //mySqlToolkit.BackupDatabase(databaseName, localDatabasePath);
            //mySqlToolkit.RestoreDatabase(databaseName, localDatabasePath);

            // postgresql
            var databaseName = "example-database";
            var localDatabasePath = "C:\\backups\\example-database.sql";

            // postgresql
            //postgreSqlToolkit.BackupDatabase(databaseName, localDatabasePath);
            //postgreSqlToolkit.RestoreDatabase(databaseName, localDatabasePath);
        }
    }
}
