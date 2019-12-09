using Abp.Application.Services;
using FengWo.Dtos;
using System.Threading.Tasks;
namespace FengWo.OrgnizationUnits
{
    /// <summary>
    /// 组织架构 Services
    /// </summary>
    public interface IOrgnizationUnitAppService : IApplicationService
    {
        OrgnizationUnitPagenationOutputDto GetOrgnizationUnitByPagenation(PagedSortedAndSearchInputDto pagedSortedAndSearchInputDto);

        Task<OrgnizationUnitDto> GetOrgnizationUnitById(int Id);

        Task<long> CreateOrUpdateOrgnizationUnit(OrgnizationUnitDto dto);

        Task DeleteOrgnizationUnitById(int id);
    }
}
