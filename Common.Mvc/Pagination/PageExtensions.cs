using System.Collections.Generic;
using System.Linq;

namespace Common.Mvc
{
    public static class PageExtensions
    {
        public static IPageList<T> ToPageList<T>(this IQueryable<T> source, int pageIndex, int pageSize)
        {
            return new PageList<T>(source, pageIndex, pageSize);
        }
        public static IPageList<T> ToPageList<T>(this IQueryable<T> source, int pageIndex, int pageSize, int totalCount)
        {
            return new PageList<T>(source, pageIndex, pageSize, totalCount);
        }
        public static IPageList<T> ToPagedList<T>(this IEnumerable<T> source, int pageIndex, int pageSize)
        {
            return new PageList<T>(source, pageIndex, pageSize);
        }
        public static IPageList<T> ToPagedList<T>(this IEnumerable<T> source, int pageIndex, int pageSize, int totalCount)
        {
            return new PageList<T>(source, pageIndex, pageSize, totalCount);
        }
    }
}
