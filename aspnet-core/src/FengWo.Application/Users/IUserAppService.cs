using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using FengWo.Dtos;
using FengWo.Roles.Dto;
using FengWo.Users.Dto;

namespace FengWo.Users
{
    public interface IUserAppService : IAsyncCrudAppService<UserDto, long, PagedUserResultRequestDto, CreateUserDto, UserDto>
    {
        UserPagenationOutputDto GetUserByPagenation(PagedSortedAndSearchInputDto pagedSortedAndSearchInputDto);
        Task<ListResultDto<RoleDto>> GetRoles();

        Task ChangeLanguage(ChangeUserLanguageDto input);
    }
}
