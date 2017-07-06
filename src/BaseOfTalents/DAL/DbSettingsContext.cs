using DAL.Exceptions;

namespace DAL
{
    public class DbSettingsContext
    {
        private static volatile DbSettingsContext instance;
        private static object syncRoot = new object();

        private string _dbInitialCatalog;
        private string _dbDataSource;
        private string _userId;
        private string _userPassword;

        public DbSettingsContext(string dbInitialCatalog, string dbDataSorce, string userId, string userPassword)
        {
            _dbInitialCatalog = dbInitialCatalog;
            _dbDataSource = dbDataSorce;
            _userId = userId;
            _userPassword = userPassword;
        }

        public static void SetInstance(string dbInitialCatalog, string dbDataSource, string userId, string userPassword)
        {
            if (instance != null)
            {
                throw new DbSettingsModificationException();
            }

            instance = new DbSettingsContext(dbInitialCatalog, dbDataSource, userId, userPassword);
        }

        public static DbSettingsContext Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                    }
                }

                return instance;
            }
        }

        public string DbInitialCatalog
        {
            get
            {
                return _dbInitialCatalog;
            }
        }

        public string DbDataSource
        {
            get
            {
                return _dbDataSource;
            }
        }

        public string UserId
        {
            get
            {
                return _userId;
            }
        }

        public string UserPassword
        {
            get
            {
                return _userPassword;
            }
        }
    }
}
