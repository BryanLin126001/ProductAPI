using System;
using System.Reflection;
using Microsoft.Data.Sqlite;
using RefactorThis.Models;

namespace RefactorThis.Helper
{
    public class BaseHelpers<T> : IHelpers<T>
    {
        private const string ConnectionString = "Data Source=App_Data/products.db";

        public virtual SqliteConnection NewConnection()
        {
            return new SqliteConnection(ConnectionString);
        }

        public virtual SqliteCommand GetSqlCommand(SqliteConnection conn)
        {
            return conn.CreateCommand();
        }

        public virtual void OpenConnection(SqliteConnection conn)
        {
            conn.Open();
        }

        public virtual void CloseDataReader(SqliteDataReader sqliteDataReader)
        {
            sqliteDataReader.Close();
        }

        public virtual void Execute(String sqlString, T obj)
        {
        }

        public virtual void SetValueToCommandParameter(SqliteCommand cmd, T obj)
        {
        }
    }
}