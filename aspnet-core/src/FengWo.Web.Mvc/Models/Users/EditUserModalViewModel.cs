using System.Collections.Generic;
using System.Linq;
using FengWo.Roles.Dto;
using FengWo.Users.Dto;

namespace FengWo.Web.Models.Users
{
    public class EditUserModalViewModel
    {
        public UserDto User { get; set; }

        /// <summary>
        /// ���н�ɫ
        /// </summary>
        public IReadOnlyList<RoleDto> Roles { get; set; }

        /// <summary>
        /// �û�����
        /// </summary>
        public int[] UserDepartments { get; set; }

        public bool UserIsInRole(RoleDto role)
        {
            return User.RoleNames != null && User.RoleNames.Any(r => r == role.NormalizedName);
        }
    }
}
