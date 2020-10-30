using RefactorThis.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RefactorThis.Helper
{
    public class HelperFactory
    {
        public static dynamic GetHelpInstance(Type t)
        {
            if (t == typeof(Product))
            {
                return new ProductHelper();
            }
            else if (t == typeof(ProductOption))
            {
                return new ProductOptionHelper();
            }

            return null;
        }
    }
}