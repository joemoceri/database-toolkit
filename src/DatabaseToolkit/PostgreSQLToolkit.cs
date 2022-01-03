using Microsoft.Extensions.Options;
using System.Diagnostics;

namespace DatabaseToolkit
{
    public interface IPostgreSQLToolkit
    {
        void RestoreDatabase(string databaseName, string localDatabasePath);
        void BackupDatabase(string databaseName, string localDatabasePath);
    }

    internal class PostgreSQLToolkit : IPostgreSQLToolkit
    {
        public IOptions<ApplicationOptions> options;

        public PostgreSQLToolkit(IOptions<ApplicationOptions> options)
        {
            this.options = options;
        }

        public void BackupDatabase(string databaseName, string localDatabasePath)
        {
            var process = new Process();
            var startInfo = new ProcessStartInfo();
            startInfo.FileName = "postgresql-backup.bat";
            var host = options.Value.PostgreSQLHost;
            var port = options.Value.PostgreSQLPort;
            var user = options.Value.PostgreSQLUser;
            var database = databaseName;
            var outputPath = localDatabasePath;

            startInfo.Arguments = $@"{host} {port} {user} {database} ""{outputPath}""";
            startInfo.CreateNoWindow = true;
            startInfo.UseShellExecute = false;
            process.StartInfo = startInfo;
            process.Start();
            process.WaitForExit();
            process.Close();
        }

        public void RestoreDatabase(string databaseName, string localDatabasePath)
        {
            var process = new Process();
            var startInfo = new ProcessStartInfo();
            startInfo.FileName = "postgresql-restore.bat";
            var host = options.Value.PostgreSQLHost;
            var port = options.Value.PostgreSQLPort;
            var user = options.Value.PostgreSQLUser;
            var database = databaseName;
            var outputPath = localDatabasePath;

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
