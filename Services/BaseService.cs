using RefactorThis.Models;
using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using RefactorThis.Helper;

namespace RefactorThis.Services
{
    public class BaseService
    {
        public virtual Guid GetItemId(SqliteDataReader rdr)
        {
            return Guid.Parse(rdr.GetString(0));
        }
    }
}