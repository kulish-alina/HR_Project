using DAL.Exceptions;

namespace DAL
{
    public class DbSettingsContext
    {
        private static volatile DbSettingsContext instance;
        private static object syncRoot = new object();

        private string _dbInitialCatalog;
        private string _dbDataSource;

        public DbSettingsContext(string dbInitialCatalog, string dbDataSorce)
        {
            _dbInitialCatalog = dbInitialCatalog;
            _dbDataSource = dbDataSorce;
        }

        public static void SetInstance(string dbInitialCatalog, string dbDataSource)
        {
            if (instance != null)
            {
                throw new DbSettingsModificationException();
            }

            instance = new DbSettingsContext(dbInitialCatalog, dbDataSource);
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
    }
}
