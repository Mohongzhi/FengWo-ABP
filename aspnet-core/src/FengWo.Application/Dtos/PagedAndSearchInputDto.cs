using FengWo.Dtos.Interface;

namespace FengWo.Dtos
{
    /// <summary>
    /// 分页, 搜索
    /// </summary>
    public class PagedAndSearchInputDto : PagedInputDto, ISearchResultRequest
    {
        /// <summary>
        /// 搜索关键字
        /// </summary>
        public string Search { get ; set ; }
    }
}
