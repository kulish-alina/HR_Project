using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services
{
    public interface IControllerService<T, E>
    {
        IEnumerable<E> GetAll();
        E GetById(int id);
        E Add(E entity);
        void Remove(int id);
        E Put(E entity);

        object Search(object searchParams);
    }
}
