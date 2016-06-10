using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Services
{
    public abstract class BaseEnumService<TEnum>
    {
        public virtual object Get(int id)
        {
            List<TEnum> enums = new List<TEnum>();
            foreach (var item in Enum.GetValues(typeof(TEnum)))
            {
                enums.Add((TEnum)item);
            }
            var objectedEnums = enums.Select(x => new { id = x, title = Enum.GetName(typeof(TEnum), x) });
            var foundedEnum = objectedEnums.FirstOrDefault(y => Convert.ToInt32(y.id) == id);
            return foundedEnum;
        }

        public virtual object GetAll()
        {
            List<TEnum> enums = new List<TEnum>();
            foreach (var item in Enum.GetValues(typeof(TEnum)))
            {
                enums.Add((TEnum)item);
            }
            var objectedEnums = enums.Select(x => new { id = x, title = Enum.GetName(typeof(TEnum), x) });
            return objectedEnums;
        }

    }
}
