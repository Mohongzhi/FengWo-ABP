using FengWo.Dtos.Interface;

namespace FengWo.Dtos
{
    /// <summary>
    /// 分页，带排序，搜索
    /// </summary>
    public class PagedSortedAndSearchInputDto : PagedAndSortedInputDto, ISearchResultRequest
    {
        /// <summary>
        /// 搜索内容
        /// </summary>
        public string Search { get; set; } = "";
    }
}
