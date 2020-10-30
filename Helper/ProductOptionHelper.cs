using Microsoft.Data.Sqlite;
using RefactorThis.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace RefactorThis.Helper
{
    public class ProductOptionHelper : BaseHelpers<ProductOption>, IHelpers<ProductOption>
    {
        public override void Execute(String SqlString, ProductOption product)
        {
            using (SqliteConnection conn = NewConnection())
            {
                OpenConnection(conn);
                using (SqliteCommand cmd = GetSqlCommand(conn))
                {
                    cmd.CommandText = SqlString;
                    SetValueToCommandParameter(cmd, product);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public override void SetValueToCommandParameter(SqliteCommand cmd, ProductOption productOption)
        {
            Type t = productOption.GetType();
            foreach (PropertyInfo property in t.GetProperties())
            {
                if (property.Name.Equals("Id") && (productOption.Id != null || productOption.Id != Guid.Empty))
                    cmd.Parameters.AddWithValue("$productOptionId", productOption.Id);
                else if (property.Name.Equals("Name") && productOption.Name != null)
                    cmd.Parameters.AddWithValue("$productOptionName", productOption.Name);
                else if (property.Name.Equals("Description") && productOption.Description != null)
                    cmd.Parameters.AddWithValue("$productOptionDesc", productOption.Description);
                else if (property.Name.Equals("ProductId") && (productOption.ProductId != null || productOption.ProductId != Guid.Empty))
                    cmd.Parameters.AddWithValue("$productId", productOption.ProductId);
            }
        }
    }
}