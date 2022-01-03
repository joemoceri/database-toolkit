using Microsoft.Extensions.Options;
using System.Diagnostics;

namespace DatabaseToolkit
{
    public interface IMongoDBToolkit
    {
        void RestoreDatabase(string localDatabasePath);
        void BackupDatabase(string databaseName, string localDatabasePath);
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
            startInfo.FileName = "mongodb-backup.bat";

            startInfo.Arguments = $@"{databaseName} ""{localDatabasePath}""";
            startInfo.CreateNoWindow = true;
            startInfo.UseShellExecute = false;
            process.StartInfo = startInfo;
            process.Start();
            process.WaitForExit();
            process.Close();
        }

        public void RestoreDatabase(string localDatabasePath)
        {
            var process = new Process();
            var startInfo = new ProcessStartInfo();
            startInfo.FileName = "mongodb-restore.bat";

            startInfo.Arguments = $@"""{localDatabasePath}""";
            startInfo.CreateNoWindow = true;
            startInfo.UseShellExecute = false;
            process.StartInfo = startInfo;
            process.Start();
            process.WaitForExit();
            process.Close();
        }
    }
}
