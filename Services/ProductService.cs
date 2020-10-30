using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore.Storage;
using RefactorThis.Helper;
using RefactorThis.Models;

namespace RefactorThis.Services
{
    public class ProductService : BaseService, IService<Product>
    {
        protected const string _selectAllSqlStr = "SELECT id FROM Products ";
        protected const string _selectByIdSqlStr = "SELECT * FROM Products WHERE id = $productId COLLATE NOCASE";
        protected const string _createItem = "INSERT INTO Products (id, name, description, price, deliveryprice) VALUES ($productId, $productName, $productDesc, $productPrice, $productDeliveryPrice)";
        protected const string _updateItem = "UPDATE Products SET name = $productName, description = $productDesc, price = $productPrice, deliveryprice = $productDeliveryPrice WHERE id = $productId COLLATE NOCASE";
        protected const string _deleteItem = "DELETE FROM Products WHERE id = $productId COLLATE NOCASE";
        private BaseHelpers<Product> _helper;

        public ProductService()
        {
            _helper = HelperFactory.GetHelpInstance(typeof(Product));
        }

        public List<Product> LoadItems(string where)
        {
            string sqlStr = _selectAllSqlStr + where;
            List<Product> Items = new List<Product>();
            using (SqliteConnection conn = _helper.NewConnection())
            {
                _helper.OpenConnection(conn);
                using (SqliteCommand cmd = _helper.GetSqlCommand(conn))
                {
                    cmd.CommandText = sqlStr;
                    SqliteDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        Guid id = GetItemId(rdr);
                        Items.Add(GetItemById(id));
                    }
                    _helper.CloseDataReader(rdr);
                }
            }
            return Items;
        }

        public Product GetItemById(Guid id)
        {
            bool IsNew = true;
            Product product = null;
            using (SqliteConnection conn = _helper.NewConnection())
            {
                _helper.OpenConnection(conn);
                using (SqliteCommand cmd = _helper.GetSqlCommand(conn))
                {
                    cmd.CommandText = _selectByIdSqlStr;
                    _helper.SetValueToCommandParameter(cmd, new Product(id));
                    SqliteDataReader rdr = cmd.ExecuteReader();
                    if (!rdr.Read())
                        return null;
                    IsNew = false;
                    product = FillProductValue(IsNew, rdr);
                    _helper.CloseDataReader(rdr);
                }
            }

            return product;
        }

        public Product FillProductValue(bool isNew, SqliteDataReader rdr)
        {
            return new Product(isNew)
            {
                Id = GetItemId(rdr),
                Name = rdr["Name"].ToString(),
                Description = (DBNull.Value == rdr["Description"]) ? null : rdr["Description"].ToString(),
                Price = decimal.Parse(rdr["Price"].ToString()),
                DeliveryPrice = decimal.Parse(rdr["DeliveryPrice"].ToString()),
            };
        }

        public void Add(Product product)
        {
            _helper.Execute(_createItem, product);
        }

        public void Update(Product product)
        {
            _helper.Execute(_updateItem, product);
        }

        public void Delete(Guid id)
        {
            _helper.Execute(_deleteItem, new Product(id));
        }

        public string GetFilterString(string value)
        {
            return $"where lower(name) like '%{value.ToLower()}%'";
        }
    }
}