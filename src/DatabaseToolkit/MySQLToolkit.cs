using Microsoft.Extensions.Options;

namespace DatabaseToolkit
{
    public interface IMySQLToolkit
    {
        void RestoreDatabase(string localDatabasePath);
        void BackupDatabase(string localDatabasePath);
    }

    internal class MySQLToolkit : IMySQLToolkit
    {
        public IOptions<ApplicationOptions> options;

        public MySQLToolkit(IOptions<ApplicationOptions> options)
        {
            this.options = options;
        }

        public void BackupDatabase(string localDatabasePath)
        {
            throw new NotImplementedException();
        }

        public void RestoreDatabase(string localDatabasePath)
        {
            throw new NotImplementedException();
        }
    }
}
