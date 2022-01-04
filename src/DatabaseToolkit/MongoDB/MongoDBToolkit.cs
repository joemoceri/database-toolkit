using Microsoft.Extensions.Options;
using System.Diagnostics;

namespace DatabaseToolkit
{
    public interface IMongoDBToolkit
    {
        void BackupDatabase(string databaseName, string localDatabasePath);
        void RestoreDatabase(string localDatabasePath);
        void BackupDatabaseWithAuthentication(string databaseName, string localDatabasePath);
        void RestoreDatabaseWithAuthentication(string localDatabasePath);
    }

    internal class MongoDBToolkit : IMongoDBToolkit
    {
        public IOptions<ApplicationOptions> options;

        public MongoDBToolkit(IOptions<ApplicationOptions> options)
        {
            this.options = options;
        }

        public void BackupDatabase(string databaseName, string localDatabasePath)
        {
            var process = new Process();
            var startInfo = new ProcessStartInfo();
            startInfo.FileName = Path.Combine("MongoDB", "mongodb-backup.bat");

            startInfo.Arguments = $@"{databaseName} ""{localDatabasePath}""";
            startInfo.CreateNoWindow = true;
            startInfo.UseShellExecute = false;
            process.StartInfo = startInfo;
            process.Start();
            process.WaitForExit();
            process.Close();
        }

        public void BackupDatabaseWithAuthentication(string databaseName, string localDatabasePath)
        {
            var process = new Process();
            var startInfo = new ProcessStartInfo();
            startInfo.FileName = Path.Combine("MongoDB", "mongodb-backup-with-auth.bat");

            startInfo.Arguments = $@"{databaseName} ""{localDatabasePath}"" {options.Value.MongoDBUser} {options.Value.MongoDBAuthenticationDatabase}";
            startInfo.CreateNoWindow = true;
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardInput = true;
            process.StartInfo = startInfo;
            process.Start();
            process.StandardInput.WriteLine(options.Value.MongoDBPassword);
            process.WaitForExit();
            process.Close();
        }

        public void RestoreDatabase(string localDatabasePath)
        {
            var process = new Process();
            var startInfo = new ProcessStartInfo();
            startInfo.FileName = Path.Combine("MongoDB", "mongodb-restore.bat");

            startInfo.Arguments = $@"""{localDatabasePath}""";
            startInfo.CreateNoWindow = true;
            startInfo.UseShellExecute = false;
            process.StartInfo = startInfo;
            process.Start();
            process.WaitForExit();
            process.Close();
        }

        public void RestoreDatabaseWithAuthentication(string localDatabasePath)
        {
            var process = new Process();
            var startInfo = new ProcessStartInfo();
            startInfo.FileName = Path.Combine("MongoDB", "mongodb-restore-with-auth.bat");

            startInfo.Arguments = $@"""{localDatabasePath}"" {options.Value.MongoDBUser} {options.Value.MongoDBAuthenticationDatabase}";
            startInfo.CreateNoWindow = true;
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardInput = true;
            process.StartInfo = startInfo;
            process.Start();
            process.StandardInput.WriteLine(options.Value.MongoDBPassword);
            process.WaitForExit();
            process.Close();
        }
    }
}
