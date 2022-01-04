using Microsoft.Extensions.Options;
using System.Diagnostics;

namespace DatabaseToolkit
{
    public interface IPostgreSQLToolkit
    {
        /// <summary>
        /// Restore a PostgreSQL database using pg_restore. Make sure the following appsettings.json properties
        /// <see cref="ApplicationOptions.PostgreSQLHost"/>, <see cref="ApplicationOptions.PostgreSQLPort"/>,
        /// and <see cref="ApplicationOptions.PostgreSQLUser"/> are set and match exactly what's in your pgpass.conf file (Windows).
        /// </summary>
        /// <param name="databaseName">The name of the database on the Postgres server we're restoring.</param>
        /// <param name="localDatabasePath">The local file path to the .sql database file where we're restoring from.</param>
        void RestoreDatabase(string databaseName, string localDatabasePath);

        /// <summary>
        /// Backup a PostgreSQL database using pg_dump. Make sure the following appsettings.json properties
        /// <see cref="ApplicationOptions.PostgreSQLHost"/>, <see cref="ApplicationOptions.PostgreSQLPort"/>,
        /// and <see cref="ApplicationOptions.PostgreSQLUser"/> are set and match exactly what's in your pgpass.conf file (Windows).
        /// </summary>
        /// <param name="databaseName">The name of the database on the Postgres server we're backing up.</param>
        /// <param name="localDatabasePath">The local file path to the .sql database file where the backup is saved.</param>
        void BackupDatabase(string databaseName, string localDatabasePath);
    }

    internal class PostgreSQLToolkit : IPostgreSQLToolkit
    {
        public IOptions<ApplicationOptions> options;

        public PostgreSQLToolkit(IOptions<ApplicationOptions> options)
        {
            this.options = options;
        }

        /// <summary>
        /// Backup a PostgreSQL database using pg_dump. Make sure the following appsettings.json properties
        /// <see cref="ApplicationOptions.PostgreSQLHost"/>, <see cref="ApplicationOptions.PostgreSQLPort"/>,
        /// and <see cref="ApplicationOptions.PostgreSQLUser"/> are set and match exactly what's in your pgpass.conf file (Windows).
        /// </summary>
        /// <param name="databaseName">The name of the database on the Postgres server we're backing up.</param>
        /// <param name="localDatabasePath">The local file path to the .sql database file where the backup is saved.</param>
        public void BackupDatabase(string databaseName, string localDatabasePath)
        {
            var process = new Process();
            var startInfo = new ProcessStartInfo();
            startInfo.FileName = Path.Combine("PostgreSQL", "postgresql-backup.bat");
            var host = options.Value.PostgreSQLHost;
            var port = options.Value.PostgreSQLPort;
            var user = options.Value.PostgreSQLUser;
            var database = databaseName;
            var outputPath = localDatabasePath;

            // use pg_dump, specifying the host, port, user, database to back up, and the output path.
            // the host, port, user, and database must be an exact match with what's inside your pgpass.conf (Windows)
            startInfo.Arguments = $@"{host} {port} {user} {database} ""{outputPath}""";
            startInfo.CreateNoWindow = true;
            startInfo.UseShellExecute = false;
            process.StartInfo = startInfo;
            process.Start();
            process.WaitForExit();
            process.Close();
        }

        /// <summary>
        /// Restore a PostgreSQL database using pg_restore. Make sure the following appsettings.json properties
        /// <see cref="ApplicationOptions.PostgreSQLHost"/>, <see cref="ApplicationOptions.PostgreSQLPort"/>,
        /// and <see cref="ApplicationOptions.PostgreSQLUser"/> are set and match exactly what's in your pgpass.conf file (Windows).
        /// </summary>
        /// <param name="databaseName">The name of the database on the Postgres server we're restoring.</param>
        /// <param name="localDatabasePath">The local file path to the .sql database file where we're restoring from.</param>
        public void RestoreDatabase(string databaseName, string localDatabasePath)
        {
            var process = new Process();
            var startInfo = new ProcessStartInfo();
            startInfo.FileName = Path.Combine("PostgreSQL", "postgresql-restore.bat");
            var host = options.Value.PostgreSQLHost;
            var port = options.Value.PostgreSQLPort;
            var user = options.Value.PostgreSQLUser;
            var database = databaseName;
            var outputPath = localDatabasePath;

            // use pg_restore, specifying the host, port, user, database to restore, and the output path.
            // the host, port, user, and database must be an exact match with what's inside your pgpass.conf (Windows)
            startInfo.Arguments = $@"{host} {port} {user} {database} ""{outputPath}""";
            startInfo.CreateNoWindow = true;
            startInfo.UseShellExecute = false;
            process.StartInfo = startInfo;
            process.Start();
            process.WaitForExit();
            process.Close();
        }
    }
}
