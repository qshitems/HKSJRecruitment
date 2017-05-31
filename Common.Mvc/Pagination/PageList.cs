using System;
using System.Collections.Generic;
using System.Linq;
namespace Common.Mvc
{
    public class PageList<T> : List<T>, IPageList<T>
    {
        public PageList(IEnumerable<T> source, int pageIndex, int pageSize)
            : this(source, pageIndex, pageSize, null)
        {
        }

        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="source">查询表达式</param>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="pageSize">每页数量</param>
        /// <param name="totalCount">总记录数量</param>
        public PageList(IEnumerable<T> source, int pageIndex, int pageSize, int? totalCount)
        {
            Initialize(source.AsQueryable(), pageIndex, pageSize, totalCount);
        }
        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="source">查询表达式</param>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="pageSize">每页数量</param>
        public PageList(IQueryable<T> source, int pageIndex, int pageSize)
            : this(source, pageIndex, pageSize, null)
        {
        }
        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="source">查询表达式</param>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="pageSize">每页数量</param>
        /// <param name="totalCount">总记录数量</param>
        public PageList(IQueryable<T> source, int pageIndex, int pageSize, int? totalCount)
        {
            Initialize(source, pageIndex, pageSize, totalCount);
        }

        #region IPageList Members

        private PageInfo _pageInfo = new PageInfo();

        public PageInfo PageInfo
        {
            get { return _pageInfo; }
        }

        #endregion

        protected void Initialize(IQueryable<T> source, int pageIndex, int pageSize, int? totalCount)
        {
            if (pageIndex < 1)
            {
                pageIndex = 1;
            }
            if (pageSize < 1)
            {
                throw new ArgumentOutOfRangeException("页尺寸不能小于1");
            }

            if (source == null)
            {
                source = new List<T>().AsQueryable();
            }

            if (totalCount.HasValue)
            {
                this._pageInfo.TotalCount = totalCount.Value;
            }
            else
            {
                this._pageInfo.TotalCount = source.Count();
            }
            this._pageInfo.PageSize = pageSize;
            this._pageInfo.PageIndex = pageIndex;
            if (this.PageInfo.TotalCount > 0)
            {
                this._pageInfo.PageCount = (int)Math.Ceiling(this._pageInfo.TotalCount / (double)this._pageInfo.PageSize);
            }
            else
            {
                this._pageInfo.PageCount = 0;
            }
            this._pageInfo.HasPreviousPage = (this._pageInfo.PageIndex > 1);
            this._pageInfo.HasNextPage = (this._pageInfo.PageIndex < this._pageInfo.PageCount);
            this._pageInfo.IsFirstPage = (this._pageInfo.PageIndex <= 1);
            this._pageInfo.IsLastPage = (this._pageInfo.PageIndex >= this._pageInfo.PageCount);

            if (this._pageInfo.TotalCount > 0)
            {
                AddRange(source.Skip((this._pageInfo.PageIndex - 1) * this._pageInfo.PageSize).Take(this._pageInfo.PageSize).ToList());
            }
        }
    }

}
