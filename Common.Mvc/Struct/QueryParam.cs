namespace Common.Mvc
{
    public class QueryParam
    {
        /// <summary>
        /// 分页参数-页大小
        /// </summary>
        public int PageSize { get; set; }
        /// <summary>
        /// 分页参数-当前页
        /// </summary>
        public int PageIndex { get; set; }
        /// <summary>
        /// 排序字段
        /// </summary>
        public string Sort { get; set; }
        /// <summary>
        /// 排序方式
        /// </summary>
        public string Order { get; set; }

        public QueryParam()
        {
            Sort = "Id";
            Order = "Desc";
            PageSize = 20;
            PageIndex = 1;
        }
    }
}