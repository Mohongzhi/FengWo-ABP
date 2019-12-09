using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using System.Linq.Dynamic.Core;
using System.Linq;
using Abp.ObjectMapping;
using FengWo.Dtos;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Abp.Application.Services;
using FengWo.DBFactory;
using Abp.Organizations;

namespace FengWo.OrgnizationUnits
{
    /// <summary>
    /// 组织架构 Services
    /// </summary>
    public class OrgnizationUnitAppService : ApplicationService, IOrgnizationUnitAppService
    {
        /// <summary>
        /// AutoMapper
        /// </summary>
        private readonly IObjectMapper _objectMapper;

        /// <summary>
        /// 组织架构 Repository
        /// </summary>
        private readonly IRepository<OrganizationUnit,long> _orgnizationUnitRepository;

        /// <summary>
        /// ADO.NET 支持
        /// </summary>
        private readonly IDBHelper _helper;

        /// <summary>
        /// IOC
        /// </summary>
        public OrgnizationUnitAppService(IObjectMapper objectMapper,
        IRepository<OrganizationUnit,long> orgnizationUnitRepository,
        IDBHelper helper)
        {
            _objectMapper = objectMapper;
            _orgnizationUnitRepository = orgnizationUnitRepository;
            _helper = helper;
        }
        /// <summary>
        /// 根据Id获取OrgnizationUnit
        /// </summary>
        /// <param name="Id">Id key</param>
        /// <returns></returns>
        public async Task<OrgnizationUnitDto> GetOrgnizationUnitById(int Id)
        {
            return _objectMapper.Map<OrgnizationUnitDto>(_orgnizationUnitRepository.Get(Id));
        }
        /// <summary>
        /// 分页获取OrgnizationUnit
        /// </summary>
        /// <param name="pagedSortedAndSearchInputDto">分页过滤排序dto</param>
        /// <returns></returns>
        public OrgnizationUnitPagenationOutputDto GetOrgnizationUnitByPagenation(PagedSortedAndSearchInputDto pagedSortedAndSearchInputDto)
        {
            var all = _orgnizationUnitRepository.GetAll();
            if (pagedSortedAndSearchInputDto.Search != null && pagedSortedAndSearchInputDto.Search.Trim().Length > 0)
            {
                all = all.Where(item => item.DisplayName.Contains(pagedSortedAndSearchInputDto.Search));//Change this Where for search
            }
            var total = all.Count();
            if (pagedSortedAndSearchInputDto.Sort.Trim().Length > 0)
            {
                all = all.OrderBy(string.Format("{0} {1}", pagedSortedAndSearchInputDto.Sort, pagedSortedAndSearchInputDto.Order));
            }
            var list = all.Skip(pagedSortedAndSearchInputDto.Offset).Take(pagedSortedAndSearchInputDto.Limit).ToDynamicList<OrganizationUnit>();
            var listDto = _objectMapper.Map<List<OrgnizationUnitDto>>(list);
            OrgnizationUnitPagenationOutputDto result = new OrgnizationUnitPagenationOutputDto() { Rows = listDto, Total = total };
            return result;
        }
        /// <summary>
        /// 创建或更新OrgnizationUnit
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public Task<long> CreateOrUpdateOrgnizationUnit(OrgnizationUnitDto dto)
        {
            var entity = _objectMapper.Map<OrganizationUnit>(dto);
            return _orgnizationUnitRepository.InsertOrUpdateAndGetIdAsync(entity);
        }
        /// <summary>
        /// 删除OrgnizationUnit
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task DeleteOrgnizationUnitById(int id)
        {
            var entity = _orgnizationUnitRepository.Get(id);
            return _orgnizationUnitRepository.DeleteAsync(entity);
        }
    }
}
