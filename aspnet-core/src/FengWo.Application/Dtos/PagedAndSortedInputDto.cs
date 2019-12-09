using FengWo.Dtos.Interface;

namespace FengWo.Dtos
{
    /// <summary>
    /// 分页, 排序
    /// </summary>
    public class PagedAndSortedInputDto : PagedInputDto, ISortedResultRequest
    {
        /// <summary>
        /// 排序列名
        /// </summary>
        public string Sort { get; set; } = "";

        /// <summary>
        /// 正序倒序
        /// </summary>
        public string Order { get; set; } = "asc";
    }
}
