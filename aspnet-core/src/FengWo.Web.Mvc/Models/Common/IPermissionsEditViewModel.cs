using System.Collections.Generic;
using FengWo.Roles.Dto;

namespace FengWo.Web.Models.Common
{
    public interface IPermissionsEditViewModel
    {
        List<FlatPermissionDto> Permissions { get; set; }
    }
}