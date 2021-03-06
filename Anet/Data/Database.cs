﻿using Microsoft.Extensions.Logging;
using System.Data;

namespace Anet.Data
{
    public class Database
    {
        internal static ILogger<Database> Logger { get; set; }

        public Database(IDbConnection connection)
        {
            Connection = connection;
        }

        public IDbConnection Connection { get; private set; }

        public IDbTransaction Transaction { get; private set; }

        public IDbTransaction BeginTransaction()
        {
            return BeginTransaction(IsolationLevel.Unspecified);
        }

        public IDbTransaction BeginTransaction(IsolationLevel il)
        {
            // Auto open connection.
            if (Connection.State == ConnectionState.Closed)
                Connection.Open();
            Transaction = Connection.BeginTransaction(il);
            return Transaction;
        }

        public void Dispose()
        {
            if (Transaction != null)
            {
                Transaction.Dispose();
                Transaction = null;
            }
            if (Connection != null)
            {
                Connection.Close();
                Connection.Dispose();
                Connection = null;
            }
        }
    }
}
