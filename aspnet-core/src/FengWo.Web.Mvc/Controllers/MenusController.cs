using Abp.AspNetCore.Mvc.Authorization;
using FengWo.Authorization;
using FengWo.Controllers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FengWo.Web.Controllers
{
    [AbpMvcAuthorize(PermissionNames.Pages_Menus)]

    public class MenusController : FengWoControllerBase
    {
        public async Task<IActionResult> Index()
        {
            return View();
        }
    }
}
