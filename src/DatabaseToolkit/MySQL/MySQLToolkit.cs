using Microsoft.Extensions.Options;
using System.Diagnostics;

namespace DatabaseToolkit
{
    public interface IMySQLToolkit
    {
        /// <summary>
        /// Restore a MySQL database using mysql. Make sure the appsettings.json properties
        /// <see cref="ApplicationOptions.MySqlDefaultsFilePath"/> points to your my.ini (Windows) file.
        /// </summary>
        /// <param name="databaseName">The name of the database on the MySQL server to restore.</param>
        /// <param name="localDatabasePath">The local path to the .sql database file that's being restored.</param>
        void RestoreDatabase(string databaseName, string localDatabasePath);

        /// <summary>
        /// Backup a MySQL database using mysqldump. Make sure the appsettings.json properties
        /// <see cref="ApplicationOptions.MySqlDefaultsFilePath"/> points to your my.ini (Windows) file
        /// and <see cref="ApplicationOptions.MySqlDumpPath" /> points to your mysqldump.exe file.
        /// </summary>
        /// <param name="databaseName">The name of the database on the MySQL server.</param>
        /// <param name="localDatabasePath">The local path to the .sql database file where the backup will be saved.</param>
        void BackupDatabase(string databaseName, string localDatabasePath);
    }

    internal class MySQLToolkit : IMySQLToolkit
    {
        public IOptions<ApplicationOptions> options;

        public MySQLToolkit(IOptions<ApplicationOptions> options)
        {
            this.options = options;
        }

        /// <summary>
        /// Backup a MySQL database using mysqldump. Make sure the appsettings.json properties
        /// <see cref="ApplicationOptions.MySqlDefaultsFilePath"/> points to your my.ini (Windows) file
        /// and <see cref="ApplicationOptions.MySqlDumpPath" /> points to your mysqldump.exe file.
        /// </summary>
        /// <param name="databaseName">The name of the database on the MySQL server.</param>
        /// <param name="localDatabasePath">The local path to the .sql database file where the backup will be saved.</param>
        public void BackupDatabase(string databaseName, string localDatabasePath)
        {
            var process = new Process();
            var startInfo = new ProcessStartInfo();

            // execute from a bat file allows the use of > and < in the arguments
            startInfo.FileName = Path.Combine("MySQL", "mysql-backup.bat");

            // call mysqldump, specifying the defaults config to use, which database to back up, and where to save the back up
            startInfo.Arguments = $@"""{options.Value.MySqlDumpPath}"" ""{options.Value.MySqlDefaultsFilePath}"" {databaseName} ""{localDatabasePath}"""; 
            startInfo.CreateNoWindow = true;
            startInfo.UseShellExecute = false;
            process.StartInfo = startInfo;
            process.Start();
            process.WaitForExit();
            process.Close();
        }

        /// <summary>
        /// Restore a MySQL database using mysql. Make sure the appsettings.json properties
        /// <see cref="ApplicationOptions.MySqlDefaultsFilePath"/> points to your my.ini (Windows) file.
        /// </summary>
        /// <param name="databaseName">The name of the database on the MySQL server to restore.</param>
        /// <param name="localDatabasePath">The local path to the .sql database file that's being restored.</param>
        public void RestoreDatabase(string databaseName, string localDatabasePath)
        {
            var process = new Process();
            var startInfo = new ProcessStartInfo();
            
            // execute from a bat file allows the use of > and < in the arguments
            startInfo.FileName = Path.Combine("MySQL", "mysql-restore.bat");

            // call mysql, specify the defaults config file, the database you're restoring, and the local path to the database you want to restore
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
