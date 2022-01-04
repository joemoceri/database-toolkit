using Microsoft.Extensions.Options;
using System.Diagnostics;

namespace DatabaseToolkit
{
    public interface IMongoDBToolkit
    {
        /// <summary>
        /// Backup a MongoDB database using mongodump compressed using gzip.
        /// </summary>
        /// <param name="databaseName">The name of the database on the server.</param>
        /// <param name="localDatabasePath">The local path to the .gz file used when saving the backup.</param>
        void BackupDatabase(string databaseName, string localDatabasePath);
        /// <summary>
        /// Restore a MongoDB database using mongorestore with a compressed gzip database backup.
        /// </summary>
        /// <param name="localDatabasePath">The local path to the .gz file we're restoring.</param>
        void RestoreDatabase(string localDatabasePath);
        /// <summary>
        /// Backup a MongoDB database using mongodump and username/password authentication compressed using gzip. Make sure the following appsettings.json properties
        /// <see cref="ApplicationOptions.MongoDBUser"/>, <see cref="ApplicationOptions.MongoDBPassword"/>,
        /// <see cref="ApplicationOptions.MongoDBAuthenticationDatabase"/> are set.
        /// </summary>
        /// <param name="databaseName">The name of the database on the server.</param>
        /// <param name="localDatabasePath">The local path to the .gz file used when saving the backup.</param>
        void BackupDatabaseWithAuthentication(string databaseName, string localDatabasePath);
        /// <summary>
        /// Restore a MongoDB database using mongorestore and username/password authentication compressed using gzip. Make sure the following appsettings.json properties
        /// <see cref="ApplicationOptions.MongoDBUser"/>, <see cref="ApplicationOptions.MongoDBPassword"/>,
        /// <see cref="ApplicationOptions.MongoDBAuthenticationDatabase"/> are set.
        /// </summary>
        /// <param name="databaseName">The name of the database on the server.</param>
        /// <param name="localDatabasePath">The local path to the .gz file we're restoring.</param>
        void RestoreDatabaseWithAuthentication(string localDatabasePath);
    }

    internal class MongoDBToolkit : IMongoDBToolkit
    {
        public IOptions<ApplicationOptions> options;

        public MongoDBToolkit(IOptions<ApplicationOptions> options)
        {
            this.options = options;
        }

        /// <summary>
        /// Backup a MongoDB database using mongodump compressed using gzip.
        /// </summary>
        /// <param name="databaseName">The name of the database on the server.</param>
        /// <param name="localDatabasePath">The local path to the .gz file used when saving the backup.</param>
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

        /// <summary>
        /// Backup a MongoDB database using mongodump and username/password authentication compressed using gzip. Make sure the following appsettings.json properties
        /// <see cref="ApplicationOptions.MongoDBUser"/>, <see cref="ApplicationOptions.MongoDBPassword"/>,
        /// <see cref="ApplicationOptions.MongoDBAuthenticationDatabase"/> are set.
        /// </summary>
        /// <param name="databaseName">The name of the database on the server.</param>
        /// <param name="localDatabasePath">The local path to the .gz file used when saving the backup.</param>
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

        /// <summary>
        /// Restore a MongoDB database using mongorestore with a compressed gzip database backup.
        /// </summary>
        /// <param name="localDatabasePath">The local path to the .gz file we're restoring.</param>
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

        /// <summary>
        /// Restore a MongoDB database using mongorestore and username/password authentication compressed using gzip. Make sure the following appsettings.json properties
        /// <see cref="ApplicationOptions.MongoDBUser"/>, <see cref="ApplicationOptions.MongoDBPassword"/>,
        /// <see cref="ApplicationOptions.MongoDBAuthenticationDatabase"/> are set.
        /// </summary>
        /// <param name="databaseName">The name of the database on the server.</param>
        /// <param name="localDatabasePath">The local path to the .gz file we're restoring.</param>
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
