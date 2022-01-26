using Microsoft.Extensions.Options;

namespace DatabaseToolkit
{
    internal class App
    {
        private readonly IOptions<ApplicationOptions> options;
        private readonly ISQLServerToolkit sqlServerToolkit;
        private readonly IMySQLToolkit mySqlToolkit;
        private readonly IPostgreSQLToolkit postgreSqlToolkit;
        private readonly IMongoDBToolkit mongoDBToolkit;
        private readonly ISQLiteToolkit sqliteToolkit;

        public App(IOptions<ApplicationOptions> options, 
            ISQLServerToolkit sqlServerToolkit, 
            IMySQLToolkit mySqlToolkit, 
            IPostgreSQLToolkit postgreSqlToolkit, 
            IMongoDBToolkit mongoDBToolkit,
            ISQLiteToolkit sqliteToolkit)
        {
            this.options = options;
            this.sqlServerToolkit = sqlServerToolkit;
            this.mySqlToolkit = mySqlToolkit;
            this.postgreSqlToolkit = postgreSqlToolkit;
            this.mongoDBToolkit = mongoDBToolkit;
            this.sqliteToolkit = sqliteToolkit;
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
            //var databaseName = "example-database";
            //var localDatabasePath = "C:\\backups\\example-database.sql";

            // postgresql
            //postgreSqlToolkit.BackupDatabase(databaseName, localDatabasePath);
            //postgreSqlToolkit.RestoreDatabase(databaseName, localDatabasePath);

            // mongodb
            //var databaseName = "test-database";
            //var localDatabasePath = "C:\\backups\\backup.gz";

            // mongodb
            //mongoDBToolkit.BackupDatabase(databaseName, localDatabasePath);
            //mongoDBToolkit.RestoreDatabase(localDatabasePath);

            // mongodb with username / password authentication
            //mongoDBToolkit.BackupDatabase(databaseName, localDatabasePath, true);
            //mongoDBToolkit.RestoreDatabase(localDatabasePath, true);

            // sqlite
            // this is the database you connect to
            // var databaseName = @"C:\path\to\your\database\database.db";

            // this is where it's saved when backing up and restored from when restoring
            // var localDatabasePath = @"C:\database.backup.db";

            // sqlite
            //sqliteToolkit.BackupDatabase(databaseName, localDatabasePath);
            //sqliteToolkit.RestoreDatabase(databaseName, localDatabasePath);

        }
    }
}
