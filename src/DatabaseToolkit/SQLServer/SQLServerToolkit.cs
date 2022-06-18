using Microsoft.Extensions.Options;
using System.Data;
using System.Data.SqlClient;

namespace DatabaseToolkit
{
    internal interface ISQLServerToolkit
    {
        /// <summary>
        /// Restore a SQL Server database by first getting the File List from SQL Server using the provided .bak file. 
        /// Then set the database to single user, perform the restore, and set it back to multi user. This uses WITH REPLACE
        /// so be aware it will overwrite the existing database.
        /// </summary>
        /// <param name="databaseName">The name of the database on server.</param>
        /// <param name="localDatabasePath">The local path to the database we're restoring.</param>
        /// <exception cref="ArgumentException">If localDatabasePath doesn't end with .bak.</exception>
        void RestoreDatabase(string databaseName, string localDatabasePath = null);
        /// <summary>
        /// Backup a SQL Server database using WITH FORMAT, be aware this will wipe out the existing media set.
        /// </summary>
        /// <param name="databaseName">The name of the database on server.</param>
        /// <param name="localDatabasePath">The local path to the database we're restoring.</param>
        /// <exception cref="ArgumentException">If localDatabasePath doesn't end with .bak.</exception>
        void BackupDatabase(string databaseName, string localDatabasePath = null);
    }

    internal class SQLServerToolkit : ISQLServerToolkit
    {
        private readonly IOptions<ApplicationOptions> options;
        private string connectionString { get { return options?.Value.GetSqlServerConnectionString; } }

        public SQLServerToolkit(IOptions<ApplicationOptions> options)
        {
            this.options = options;
        }

        /// <summary>
        /// Restore a SQL Server database by first getting the File List from SQL Server using the provided .bak file. 
        /// Then set the database to single user, perform the restore, and set it back to multi user. This uses WITH REPLACE
        /// so be aware it will overwrite the existing database.
        /// </summary>
        /// <param name="databaseName">The name of the database on server.</param>
        /// <param name="localDatabasePath">The local path to the database we're restoring.</param>
        /// <exception cref="ArgumentException">If localDatabasePath doesn't end with .bak.</exception>
        public void RestoreDatabase(string databaseName, string localDatabasePath = null)
        {
            // use the default sql server base path from appsettings.json if localDatabasePath is null
            if (localDatabasePath == null)
            {
                localDatabasePath = Path.Combine(options.Value.SqlServerBasePath, "Backup", $"{databaseName}.bak");
            }
            // otherwise check if it ends with .bak
            else if (!localDatabasePath.EndsWith(".bak"))
            {
                throw new ArgumentException("localDatabasePath must end with .bak.");
            }

            // get file list data
            var fileList = GetDatabaseFileList(localDatabasePath);

            RestoreDatabase(localDatabasePath, fileList.DataName, fileList.LogName);

            DatabaseFileList GetDatabaseFileList(string localDatabasePath)
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    var sql = @"RESTORE FILELISTONLY FROM DISK = @localDatabasePath";
                    connection.Open();

                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.CommandType = CommandType.Text;
                        command.Parameters.AddWithValue("@localDatabasePath", localDatabasePath);

                        using (var reader = command.ExecuteReader())
                        {
                            var result = new DatabaseFileList();
                            while (reader.Read())
                            {
                                var type = reader["Type"].ToString();
                                switch (type)
                                {
                                    case "D":
                                        result.DataName = reader["LogicalName"].ToString();
                                        break;
                                    case "L":
                                        result.LogName = reader["LogicalName"].ToString();
                                        break;
                                }
                            }

                            return result;
                        }
                    }
                }
            }

            void RestoreDatabase(string localDatabasePath, string fileListDataName, string fileListLogName)
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    try
                    {
                        // set database to single user
                        var sql = @"
                            declare @database varchar(max) = quotename(@databaseName)
                            EXEC('ALTER DATABASE ' + @database + ' SET SINGLE_USER WITH ROLLBACK IMMEDIATE')";
                        using (var command = new SqlCommand(sql, connection))
                        {
                            command.CommandType = CommandType.Text;
                            command.Parameters.AddWithValue("@databaseName", databaseName);

                            command.ExecuteNonQuery();
                        }

                        // execute the database restore
                        var dataPath = Path.Combine(options.Value.SqlServerBasePath, "DATA");
                        var fileListDataPath = Path.Combine(dataPath, $"{fileListDataName}.mdf");
                        var fileListLogPath = Path.Combine(dataPath, $"{fileListLogName}.ldf");

                        sql = @"
                                    RESTORE DATABASE @databaseName 
                                    FROM DISK = @localDatabasePath 
                                    WITH REPLACE,
                                    MOVE @fileListDataName to @fileListDataPath,
                                    MOVE @fileListLogName to @fileListLogPath";

                        using (var command = new SqlCommand(sql, connection))
                        {
                            command.CommandTimeout = 7200;
                            command.CommandType = CommandType.Text;
                            command.Parameters.AddWithValue("@databaseName", fileListDataName);
                            command.Parameters.AddWithValue("@localDatabasePath", localDatabasePath);
                            command.Parameters.AddWithValue("@fileListDataName", fileListDataName);
                            command.Parameters.AddWithValue("@fileListDataPath", fileListDataPath);
                            command.Parameters.AddWithValue("@fileListLogName", fileListLogName);
                            command.Parameters.AddWithValue("@fileListLogPath", fileListLogPath);

                            command.ExecuteNonQuery();
                        }

                        // set database to multi user
                        sql = @"
                            declare @database varchar(max) = quotename(@databaseName)
                            EXEC('ALTER DATABASE ' + @database + ' SET MULTI_USER')";
                        using (var command = new SqlCommand(sql, connection))
                        {
                            command.CommandType = CommandType.Text;
                            command.Parameters.AddWithValue("@databaseName", databaseName);

                            command.ExecuteNonQuery();
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }
                }
            }
        }

        /// <summary>
        /// Backup a SQL Server database using WITH FORMAT, be aware this will wipe out the existing media set.
        /// </summary>
        /// <param name="databaseName">The name of the database on server.</param>
        /// <param name="localDatabasePath">The local path to the database we're restoring.</param>
        /// <exception cref="ArgumentException">If localDatabasePath doesn't end with .bak.</exception>
        public void BackupDatabase(string databaseName, string localDatabasePath = null)
        {
            // use the default sql server base path from appsettings.json if localDatabasePath is null
            if (localDatabasePath == null)
            {
                localDatabasePath = Path.Combine(options.Value.SqlServerBasePath, "Backup", $"{databaseName}.bak");
            }
            // otherwise check if it ends with .bak
            else if (!localDatabasePath.EndsWith(".bak"))
            {
                throw new ArgumentException("localDatabasePath must end with .bak.");
            }

            var formatMediaName = $"DatabaseToolkitBackup_{databaseName}";
            var formatName = $"Full Backup of {databaseName}";

            using (var connection = new SqlConnection(connectionString))
            {
                var sql = @"BACKUP DATABASE @databaseName
                            TO DISK = @localDatabasePath
                            WITH FORMAT,
	                            MEDIANAME = @formatMediaName,
                                NAME = @formatName";

                connection.Open();

                using (var command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    command.CommandTimeout = 7200;
                    command.Parameters.AddWithValue("@databaseName", databaseName);
                    command.Parameters.AddWithValue("@localDatabasePath", localDatabasePath);
                    command.Parameters.AddWithValue("@formatMediaName", formatMediaName);
                    command.Parameters.AddWithValue("@formatName", formatName);

                    command.ExecuteNonQuery();
                }
            }
        }
    }
}