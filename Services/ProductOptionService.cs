using RefactorThis.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RefactorThis.Helper;
using Microsoft.Data.Sqlite;
using System.Reflection;

namespace RefactorThis.Services
{
    public class ProductOptionService : BaseService, IService<ProductOption>
    {
        protected const string _selectAllSqlStr = "SELECT id FROM productoptions ";
        protected const string _selectByIdSqlStr = "SELECT * FROM productoptions WHERE id = $productOptionId COLLATE NOCASE";
        protected const string _createItem = "INSERT INTO productoptions (id, productid, name, description) VALUES ($productOptionId, $productId, $productOptionName, $productOptionDesc)";
        protected const string _updateItem = "UPDATE productoptions SET name = $productOptionName, description = $productOptionDesc WHERE id = $productOptionId COLLATE NOCASE";
        protected const string _deleteItem = "DELETE FROM productoptions WHERE id = $productOptionId COLLATE NOCASE";
        private BaseHelpers<ProductOption> _helper;

        public ProductOptionService()
        {
            _helper = HelperFactory.GetHelpInstance(typeof(ProductOption));
        }

        public List<ProductOption> LoadItems(string where)
        {
            string sqlStr = _selectAllSqlStr + where;
            List<ProductOption> Items = new List<ProductOption>();

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

        public ProductOption GetItemById(Guid id)
        {
            bool IsNew = true;
            ProductOption productOption = null;
            using (SqliteConnection conn = _helper.NewConnection())
            {
                _helper.OpenConnection(conn);
                using (SqliteCommand cmd = _helper.GetSqlCommand(conn))
                {
                    cmd.CommandText = _selectByIdSqlStr;

                    _helper.SetValueToCommandParameter(cmd, new ProductOption(id));
                    SqliteDataReader rdr = cmd.ExecuteReader();
                    if (!rdr.Read())
                        return null;

                    IsNew = false;
                    productOption = FillProductOptionValue(IsNew, rdr);
                    _helper.CloseDataReader(rdr);
                }
            }
            return productOption;
        }

        public ProductOption FillProductOptionValue(bool isNew, SqliteDataReader rdr)
        {
            return new ProductOption(isNew)
            {
                Id = GetItemId(rdr),
                ProductId = Guid.Parse(rdr["ProductId"].ToString()),
                Name = rdr["Name"].ToString(),
                Description = (DBNull.Value == rdr["Description"]) ? null : rdr["Description"].ToString()
            };
        }

        public void Add(ProductOption productOption)
        {
            _helper.Execute(_createItem, productOption);
        }

        public void Update(ProductOption productOption)
        {
            _helper.Execute(_updateItem, productOption);
        }

        public void Delete(Guid id)
        {
            _helper.Execute(_deleteItem, new ProductOption(id));
        }

        public string GetFilterString(string value)
        {
            return $"WHERE productid = '{value}' COLLATE NOCASE";
        }
    }
}