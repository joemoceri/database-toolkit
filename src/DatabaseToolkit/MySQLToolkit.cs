using Microsoft.Extensions.Options;
using System.Diagnostics;

namespace DatabaseToolkit
{
    public interface IMySQLToolkit
    {
        void RestoreDatabase(string databaseName, string localDatabasePath);
        void BackupDatabase(string databaseName, string localDatabasePath);
    }

    internal class MySQLToolkit : IMySQLToolkit
    {
        public IOptions<ApplicationOptions> options;

        public MySQLToolkit(IOptions<ApplicationOptions> options)
        {
            this.options = options;
        }

        public void BackupDatabase(string databaseName, string localDatabasePath)
        {
            var process = new Process();
            var startInfo = new ProcessStartInfo();
            startInfo.FileName = options.Value.MySqlDumpPath;
            startInfo.Arguments = $@"--defaults-file=""{options.Value.MySqlDefaultsFilePath}"" {databaseName} -r {localDatabasePath}";
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
            startInfo.FileName = "mysql-restore.bat";
            startInfo.Arguments = $@"""{options.Value.MySqlDefaultsFilePath}"" {databaseName} ""{localDatabasePath}""";
            startInfo.CreateNoWindow = true;
            startInfo.UseShellExecute = false;
            process.StartInfo = startInfo;
            process.Start();
            process.WaitForExit();
            process.Close();
        }
    }
}
