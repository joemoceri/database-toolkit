using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseToolkit
{
    public interface ISQLiteToolkit
    {
        void RestoreDatabase(string databaseName, string localDatabasePath);
        void BackupDatabase(string databaseName, string localDatabasePath);
    }
    internal class SQLiteToolkit : ISQLiteToolkit
    {
        public IOptions<ApplicationOptions> options;

        public SQLiteToolkit(IOptions<ApplicationOptions> options)
        {
            this.options = options;
        }

        public void BackupDatabase(string databaseName, string localDatabasePath)
        {
            var process = new Process();
            var startInfo = new ProcessStartInfo();
            startInfo.FileName = Path.Combine("SQLite", "sqlite-backup.bat");

            startInfo.Arguments = $@"""{databaseName}"" ""{localDatabasePath.Replace(@"\", @"\\")}""";
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
            startInfo.FileName = Path.Combine("SQLite", "sqlite-restore.bat");

            startInfo.Arguments = $@"""{databaseName}"" ""{localDatabasePath.Replace(@"\", @"\\")}""";
            startInfo.CreateNoWindow = true;
            startInfo.UseShellExecute = false;
            process.StartInfo = startInfo;
            process.Start();
            process.WaitForExit();
            process.Close();
        }
    }
}
