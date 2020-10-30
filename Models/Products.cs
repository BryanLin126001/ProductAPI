using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace RefactorThis.Models
{
    public class Products
    {
        public List<Product> Items { get; private set; }

        public Products(List<Product> items)
        {
            Items = items;
        }
    }

    public class Product
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public decimal DeliveryPrice { get; set; }

        [JsonIgnore] public bool IsNew { get; }

        public Product()
        {
            Id = Guid.NewGuid();
            IsNew = true;
        }

        public Product(bool IsNew)
        {
            this.IsNew = IsNew;
        }

        public Product(Guid Id)
        {
            this.Id = Id;
            this.Price = -1;
            this.DeliveryPrice = -1;
        }
    }
}