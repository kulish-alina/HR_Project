using System.Collections;
using System.Collections.Generic;

namespace BaseOfTalents.DAL.Extensions
{
    public interface IPagedList
    {
        int PageIndex { get; }

        int PageSize { get; }

        int TotalCount { get; }

        IList List { get; }
    }

    public interface IPagedList<T> : IPagedList
    {
        new IList<T> List { get; }
    }
}