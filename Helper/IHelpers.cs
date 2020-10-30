using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RefactorThis.Helper
{
    public interface IHelpers<T>
    {
        void Execute(String Sqlstring, T obj);
    }
}