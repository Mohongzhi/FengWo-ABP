using Abp.Application.Services;
using FengWo.Dtos;
using FengWo.Menus.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FengWo.Menus
{
    public interface IMenuAppService : IApplicationService
    {
        MenusOutputDto GetMenus(PagedSortedAndSearchInputDto pagedSortedAndSearchInputDto);

        Task<MenuDto> GetMenuById(int Id);

        Task<int> CreateOrUpdateMenu(MenuDto dto);

        Task DeleteMenuById(int id);
    }
}
