﻿using System;
using System.Data.SqlClient;

namespace DAL
{
    public static class ConnectionHelper
    {
        public static string CreateConnectionString(string dataSource, string initialCatalog)
        {
            if (string.IsNullOrEmpty(dataSource))
            {
                throw new ArgumentException("DataSource can't be null or empty");
            }

            if (string.IsNullOrEmpty(initialCatalog))
            {
                throw new ArgumentException("InitialCatalog can't be null or empty");
            }

            SqlConnectionStringBuilder sqlBuilder = new SqlConnectionStringBuilder();
            sqlBuilder.DataSource = dataSource;
            sqlBuilder.InitialCatalog = initialCatalog;
            sqlBuilder.IntegratedSecurity = true;

            return sqlBuilder.ConnectionString;
        }
    }

}
