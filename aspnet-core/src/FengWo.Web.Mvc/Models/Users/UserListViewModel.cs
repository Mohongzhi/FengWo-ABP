using System.Collections.Generic;
using FengWo.Roles.Dto;
using FengWo.Users.Dto;

namespace FengWo.Web.Models.Users
{
    public class UserListViewModel
    {
        public IReadOnlyList<UserDto> Users { get; set; }

        public IReadOnlyList<RoleDto> Roles { get; set; }

    }
}
