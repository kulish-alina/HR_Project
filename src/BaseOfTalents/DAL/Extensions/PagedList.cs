using System.Collections;
using System.Collections.Generic;

namespace BaseOfTalents.DAL.Extensions
{
    public class PagedList<T> : IPagedList<T>
    {
        public PagedList(
            IList<T> list,
            int pageIndex,
            int pageSize,
            int totalCount)
        {
            List = list;
            PageIndex = pageIndex;
            PageSize = pageSize;
            TotalCount = totalCount;
        }

        public int PageIndex { get; }

        public int PageSize { get; }

        public int TotalCount { get; }

        public IList<T> List { get; }

        IList IPagedList.List
        {
            get { return (IList)List; }
        }
    }
}