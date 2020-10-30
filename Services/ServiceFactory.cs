using RefactorThis.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RefactorThis.Services
{
    public class ServiceFactory
    {
        public static dynamic GetServiceInstance(Type t)
        {
            if (t == typeof(Product))
            {
                return new ProductService();
            }
            else if (t == typeof(ProductOption))
            {
                return new ProductOptionService();
            }

            return null;
        }
    }
}