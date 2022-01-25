using Microsoft.Extensions.Options;
using System.Diagnostics;

namespace DatabaseToolkit
{
    public interface IMongoDBToolkit
    {
        /// <summary>
        /// Backup a MongoDB database using mongodump and optionally with username/password authentication, compressed using gzip. Make sure the following appsettings.json properties
        /// <see cref="ApplicationOptions.MongoDBUser"/>, <see cref="ApplicationOptions.MongoDBPassword"/>,
        /// <see cref="ApplicationOptions.MongoDBAuthenticationDatabase"/> are set when withAuthentication is set to true.
        /// </summary>
        /// <param name="databaseName">The name of the database on the server.</param>
        /// <param name="localDatabasePath">The local path to the .gz file used when saving the backup.</param>
        /// <param name="withAuthentication">Whether to use authentication or not. Default is false.</param>
        void BackupDatabase(string databaseName, string localDatabasePath, bool withAuthentication = false);
        /// <summary>
        /// Restore a MongoDB database using mongorestore and optionally with username/password authentication, compressed using gzip. Make sure the following appsettings.json properties
        /// <see cref="ApplicationOptions.MongoDBUser"/>, <see cref="ApplicationOptions.MongoDBPassword"/>,
        /// <see cref="ApplicationOptions.MongoDBAuthenticationDatabase"/> are set when withAuthentication is set to true.
        /// </summary>
        /// <param name="databaseName">The name of the database on the server.</param>
        /// <param name="localDatabasePath">The local path to the .gz file we're restoring.</param>
        /// <param name="withAuthentication">Whether to use authentication or not. Default is false.</param>
        void RestoreDatabase(string localDatabasePath, bool withAuthentication = false);
    }

    internal class MongoDBToolkit : IMongoDBToolkit
    {
        public IOptions<ApplicationOptions> options;

        public MongoDBToolkit(IOptions<ApplicationOptions> options)
        {
            this.options = options;
        }

        /// <summary>
        /// Backup a MongoDB database using mongodump and optionally with username/password authentication, compressed using gzip. Make sure the following appsettings.json properties
        /// <see cref="ApplicationOptions.MongoDBUser"/>, <see cref="ApplicationOptions.MongoDBPassword"/>,
        /// <see cref="ApplicationOptions.MongoDBAuthenticationDatabase"/> are set when withAuthentication is set to true.
        /// </summary>
        /// <param name="databaseName">The name of the database on the server.</param>
        /// <param name="localDatabasePath">The local path to the .gz file used when saving the backup.</param>
        /// <param name="withAuthentication">Whether to use authentication or not. Default is false.</param>
        public void BackupDatabase(string databaseName, string localDatabasePath, bool withAuthentication = false)
        {
            var process = new Process();
            var startInfo = new ProcessStartInfo();

            var fileName = "mongodb-backup.bat";

            if (withAuthentication)
            {
                fileName = "mongodb-backup-with-auth.bat";
            }

            startInfo.FileName = Path.Combine("MongoDB", fileName);

            var arguments = $@"{databaseName} ""{localDatabasePath}""";

            if (withAuthentication)
            {
                arguments += $" {options.Value.MongoDBUser} {options.Value.MongoDBAuthenticationDatabase}";
                startInfo.RedirectStandardInput = true;
            }

            startInfo.Arguments = arguments;
            startInfo.CreateNoWindow = true;
            startInfo.UseShellExecute = false;
            process.StartInfo = startInfo;
            process.Start();
            if (withAuthentication)
            {
                process.StandardInput.WriteLine(options.Value.MongoDBPassword);
            }
            process.WaitForExit();
            process.Close();
        }

        /// <summary>
        /// Restore a MongoDB database using mongorestore and optionally with username/password authentication, compressed using gzip. Make sure the following appsettings.json properties
        /// <see cref="ApplicationOptions.MongoDBUser"/>, <see cref="ApplicationOptions.MongoDBPassword"/>,
        /// <see cref="ApplicationOptions.MongoDBAuthenticationDatabase"/> are set if withAuthentication is set to true.
        /// </summary>
        /// <param name="databaseName">The name of the database on the server.</param>
        /// <param name="localDatabasePath">The local path to the .gz file we're restoring.</param>
        /// <param name="withAuthentication">Whether to use authentication or not. Default is false.</param>
        public void RestoreDatabase(string localDatabasePath, bool withAuthentication = false)
        {
            var process = new Process();
            var startInfo = new ProcessStartInfo();
            var fileName = "mongodb-restore.bat";

            if (withAuthentication)
            {
                fileName = "mongodb-restore-with-auth.bat";
            }

            startInfo.FileName = Path.Combine("MongoDB", fileName);

            arguments = $@"""{localDatabasePath}""";
            
            if (withAuthentication)
            {
                arguments += $" {options.Value.MongoDBUser} {options.Value.MongoDBAuthenticationDatabase}";
                startInfo.RedirectStandardInput = true;
            }

            startInfo.Arguments = arguments;
            
            startInfo.CreateNoWindow = true;
            startInfo.UseShellExecute = false;
            process.StartInfo = startInfo;
            process.Start();
            
            if (withAuthentication)
            {
                process.StandardInput.WriteLine(options.Value.MongoDBPassword);
            }

            process.WaitForExit();
            process.Close();
        }
    }
}
