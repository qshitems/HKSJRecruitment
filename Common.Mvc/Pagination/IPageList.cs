using System.Collections.Generic;

namespace Common.Mvc
{
    public interface IPageList<T> : IList<T>
    {
        PageInfo PageInfo { get; }
    }
}
