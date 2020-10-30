using RefactorThis.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RefactorThis.Services
{
    internal interface IService<T>
    {
        List<T> LoadItems(string where);

        T GetItemById(Guid id);

        void Add(T item);

        void Update(T item);

        void Delete(Guid id);

        string GetFilterString(string value);
    }
}