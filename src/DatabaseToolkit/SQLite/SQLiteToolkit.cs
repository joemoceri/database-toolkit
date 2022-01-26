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
        /// <summary>
        /// Backup a sqlite database using sqlite3 and the .backup dot command.
        /// </summary>
        /// <param name="databaseName">This is the database you connect to including extension.</param>
        /// <param name="localDatabasePath">The path where the backup is stored including extension.</param>
        void BackupDatabase(string databaseName, string localDatabasePath);

        /// <summary>
        /// Restore a sqlite database using sqlite3 and the .restore dot command.
        /// </summary>
        /// <param name="databaseName">This is the database you connect to including extension.</param>
        /// <param name="localDatabasePath">The path where the backup is stored including extension.</param>
        void RestoreDatabase(string databaseName, string localDatabasePath);
    }
    internal class SQLiteToolkit : ISQLiteToolkit
    {
        public IOptions<ApplicationOptions> options;

        public SQLiteToolkit(IOptions<ApplicationOptions> options)
        {
            this.options = options;
        }

        /// <summary>
        /// Backup a sqlite database using sqlite3 and the .backup dot command.
        /// </summary>
        /// <param name="databaseName">This is the database you connect to including extension.</param>
        /// <param name="localDatabasePath">The path where the backup is stored including extension.</param>
        public void BackupDatabase(string databaseName, string localDatabasePath)
        {
            var process = new Process();
            var startInfo = new ProcessStartInfo();
            startInfo.FileName = Path.Combine("SQLite", "sqlite-backup.bat");

            // sqlite needs to have two slashes instead of one for the path it uses inside sqlite3
            startInfo.Arguments = $@"""{databaseName}"" ""{localDatabasePath.Replace(@"\", @"\\")}""";
            startInfo.CreateNoWindow = true;
            startInfo.UseShellExecute = false;
            process.StartInfo = startInfo;
            process.Start();
            process.WaitForExit();
            process.Close();
        }

        /// <summary>
        /// Restore a sqlite database using sqlite3 and the .restore dot command.
        /// </summary>
        /// <param name="databaseName">This is the database you connect to including extension.</param>
        /// <param name="localDatabasePath">The path where the backup is stored including extension.</param>
        public void RestoreDatabase(string databaseName, string localDatabasePath)
        {
            var process = new Process();
            var startInfo = new ProcessStartInfo();
            startInfo.FileName = Path.Combine("SQLite", "sqlite-restore.bat");

            // sqlite needs to have two slashes instead of one for the path it uses inside sqlite3
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
