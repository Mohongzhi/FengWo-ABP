using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Abp.Application.Services.Dto;
using Abp.AspNetCore.Mvc.Authorization;
using FengWo.Authorization;
using FengWo.Controllers;
using FengWo.Users;
using FengWo.Web.Models.Users;
using FengWo.Users.Dto;
using Abp.Domain.Repositories;
using Abp.AutoMapper;
using System.Collections.Generic;
using System.Linq;
using FengWo.Authorization.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Abp.Authorization.Users;
using System;
using Newtonsoft.Json.Linq;
using FengWo.DBFactory;

namespace FengWo.Web.Controllers
{
    [AbpMvcAuthorize(PermissionNames.Pages_Users)]
    public class UsersController : FengWoControllerBase
    {
        private IDBHelper helper;
        private readonly IRepository<User, long> _userRepository;
        private readonly IRepository<UserLogin, long> _userLoginRepository;
        private readonly IUserAppService _userAppService;

        public UsersController(IDBHelper helper,
            IUserAppService userAppService,
            IRepository<User, long> userRepository,
            IRepository<UserLogin, long> userLoginRepository)
        {
            this.helper = helper;
            _userAppService = userAppService;
            _userLoginRepository = userLoginRepository;
            _userRepository = userRepository;
        }

        public async Task<ActionResult> Index()
        {
            //var users = (await _userAppService.GetAll(new PagedUserResultRequestDto { MaxResultCount = int.MaxValue })).Items; // Paging not implemented yet
            var roles = (await _userAppService.GetRoles()).Items;
            var model = new UserListViewModel
            {
//Users = users,
                Roles = roles,
            };
            return View(model);
        }

        public async Task<ActionResult> EditUserModal(long userId)
        {
            var user = await _userAppService.Get(new EntityDto<long>(userId));
            var roles = (await _userAppService.GetRoles()).Items;
            var model = new EditUserModalViewModel
            {
                User = user,
                Roles = roles,
            };
            return View("_EditUserModal", model);
        }
    }
}
