
namespace Souing.Net.Model.Base.Domain
{
    /// <summary>
    /// 功能描述：PagerInfo 
    /// 创 建 者：Administrator  2015/3/4 11:41:31
    /// </summary>
    public class PagerInfo
    {
        /// <summary>
        /// 每页显示的条数 必填
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 请求查询的页数 必填
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// 返回总条数 
        /// </summary>
        public long TotalCount { get; set; }

        /// <summary>
        /// 返回总页数 
        /// </summary>
        public long TotalPage { get; set; }
    }
}
