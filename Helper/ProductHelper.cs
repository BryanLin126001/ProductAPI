using Microsoft.Data.Sqlite;
using RefactorThis.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace RefactorThis.Helper
{
    public class ProductHelper : BaseHelpers<Product>, IHelpers<Product>
    {
        public override void Execute(String SqlString, Product product)
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

        public override void SetValueToCommandParameter(SqliteCommand cmd, Product product)
        {
            Type t = product.GetType();
            foreach (PropertyInfo property in t.GetProperties())
            {
                if (property.Name.Equals("Id") && (product.Id != null || product.Id != Guid.Empty))
                    cmd.Parameters.AddWithValue("$productId", product.Id);
                else if (property.Name.Equals("Name") && product.Name != null)
                    cmd.Parameters.AddWithValue("$productName", product.Name);
                else if (property.Name.Equals("Description") && product.Description != null)
                    cmd.Parameters.AddWithValue("$productDesc", product.Description);
                else if (property.Name.Equals("Price") && product.Price != -1)
                    cmd.Parameters.AddWithValue("$productPrice", product.Price);
                else if (property.Name.Equals("DeliveryPrice") && product.DeliveryPrice != -1)
                    cmd.Parameters.AddWithValue("$productDeliveryPrice", product.DeliveryPrice);
            }
        }
    }
}