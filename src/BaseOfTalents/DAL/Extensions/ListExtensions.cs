using System.Collections.Generic;
using System.Linq;

namespace BaseOfTalents.DAL.Extensions
{
    public static class ListExtensions
    {
        public static IPagedList<T> ToPagedList<T>(
            this IList<T> list,
            int pageIndex,
            int pageSize,
            int totalCount)
        {
            return new PagedList<T>(list, pageIndex, pageSize, totalCount);
        }

        public static IPagedList<T> TakePage<T>(
            this IList<T> items,
            int pageIndex,
            int pageSize)
        {
            var collection = items
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .ToArray();
            return new PagedList<T>(collection, pageIndex, pageSize, items.Count);
        }
    }
}