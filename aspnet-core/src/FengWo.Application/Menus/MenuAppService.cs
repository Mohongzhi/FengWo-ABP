using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using FengWo.Authorization.Menus;
using FengWo.Dtos;
using FengWo.Menus.Dto;
using System.Linq.Dynamic.Core;
using System.Reflection;
using System.Linq;
using Abp.AutoMapper;
using Abp.ObjectMapping;

namespace FengWo.Menus
{
    /// <summary>
    /// 菜单服务
    /// </summary>
    public class MenuAppService : IMenuAppService
    {
        /// <summary>
        /// AutoMapper
        /// </summary>
        private readonly IObjectMapper _objectMapper;

        private readonly IRepository<Menu> _repository;

        /// <summary>
        /// IOC注入
        /// </summary>
        /// <param name="repository"></param>
        public MenuAppService(IRepository<Menu> repository,
          IObjectMapper objectMapper)
        {
            _repository = repository;
            _objectMapper = objectMapper;
        }

        /// <summary>
        /// 根据Id获取菜单
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<MenuDto> GetMenuById(int Id)
        {
            return _objectMapper.Map<MenuDto>(_repository.Get(Id));
        }

        /// <summary>
        /// 获取所有菜单
        /// </summary>
        /// <param name="pagedSortedAndSearchInputDto">分页过滤排序dto</param>
        /// <returns></returns>
        public MenusOutputDto GetMenus(PagedSortedAndSearchInputDto pagedSortedAndSearchInputDto)
        {
            var all = _repository.GetAll();

            if (pagedSortedAndSearchInputDto.Search != null && pagedSortedAndSearchInputDto.Search.Trim().Length > 0)
            {
                all = all.Where(item => item.Name.Contains(pagedSortedAndSearchInputDto.Search));//.Where($"Name.Contains(\"@0\")", pagedSortedAndSearchInputDto.Search);
            }

            var total = all.Count();

            if (pagedSortedAndSearchInputDto.Sort.Trim().Length > 0)
            {
                //var orderExpression = 
                all = all.OrderBy(string.Format("{0} {1}", pagedSortedAndSearchInputDto.Sort, pagedSortedAndSearchInputDto.Order));
            }

            var list = all.Skip(pagedSortedAndSearchInputDto.Offset).Take(pagedSortedAndSearchInputDto.Limit).ToDynamicList<Menu>();

            var listDto = _objectMapper.Map<List<MenuDto>>(list);

            MenusOutputDto menusOutputDto = new MenusOutputDto() { Rows = listDto, Total = total };
            //menusOutputDto.Rows = listDto;
            //menusOutputDto.Total = total;

            return menusOutputDto;
        }

        /// <summary>
        /// 创建或修改菜单
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public Task<int> CreateOrUpdateMenu(MenuDto dto)
        {
            var entity = _objectMapper.Map<Menu>(dto);

            var hasParent = _repository.FirstOrDefault(p => p.Name == dto.ParentMenuName);
            if (hasParent != null)
            {
                entity.ParentMenuId = hasParent.Id;
            }

            return _repository.InsertOrUpdateAndGetIdAsync(entity);
        }

        /// <summary>
        /// 删除菜单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task DeleteMenuById(int id)
        {
            var menu = _repository.Get(id);
            return _repository.DeleteAsync(menu);
        }

    }
}
