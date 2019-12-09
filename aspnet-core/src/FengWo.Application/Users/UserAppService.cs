using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.AutoMapper;
using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.IdentityFramework;
using Abp.Linq.Extensions;
using Abp.Localization;
using Abp.ObjectMapping;
using Abp.Runtime.Session;
using Abp.UI;
using FengWo.Authorization;
using FengWo.Authorization.Accounts;
using FengWo.Authorization.Roles;
using FengWo.Authorization.Users;
using FengWo.DBFactory;
using FengWo.Dtos;
using FengWo.Roles.Dto;
using FengWo.Users.Dto;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace FengWo.Users
{
    [AbpAuthorize(PermissionNames.Pages_Users)]
    public class UserAppService : AsyncCrudAppService<User, UserDto, long, PagedUserResultRequestDto, CreateUserDto, UserDto>, IUserAppService
    {
        private readonly UserManager _userManager;
        private readonly RoleManager _roleManager;
        private readonly IRepository<Role> _roleRepository;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IAbpSession _abpSession;
        private readonly LogInManager _logInManager;
        private readonly IDBHelper _helper;
        private readonly IRepository<User,long> _userRepository;
        private readonly IObjectMapper _objectMapper;

        public UserAppService(
            IRepository<User, long> repository,
            UserManager userManager,
            RoleManager roleManager,
            IRepository<Role> roleRepository,
            IPasswordHasher<User> passwordHasher,
            IAbpSession abpSession,
            LogInManager logInManager,
            IRepository<User, long> userRepository,
            IObjectMapper objectMapper,
            IDBHelper helper)
            : base(repository)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _roleRepository = roleRepository;
            _passwordHasher = passwordHasher;
            _abpSession = abpSession;
            _logInManager = logInManager;
            _helper = helper;
            _userRepository = userRepository;
            _objectMapper = objectMapper;
        }

        /// <summary>
        /// 分页获取User
        /// </summary>
        /// <param name="pagedSortedAndSearchInputDto">分页过滤排序dto</param>
        /// <returns></returns>
        public UserPagenationOutputDto GetUserByPagenation(PagedSortedAndSearchInputDto pagedSortedAndSearchInputDto)
        {
            var all = _userRepository.GetAll();
            if (pagedSortedAndSearchInputDto.Search != null && pagedSortedAndSearchInputDto.Search.Trim().Length > 0)
            {
                all = all.Where(item => item.Name.Contains(pagedSortedAndSearchInputDto.Search));
            }
            var total = all.Count();
            if (pagedSortedAndSearchInputDto.Sort.Trim().Length > 0)
            {
                all = all.OrderBy(string.Format("{0} {1}", pagedSortedAndSearchInputDto.Sort, pagedSortedAndSearchInputDto.Order));
            }
            var list = all.Skip(pagedSortedAndSearchInputDto.Offset).Take(pagedSortedAndSearchInputDto.Limit).ToDynamicList<User>();
            var listDto = _objectMapper.Map<List<UserDto>>(list);
            UserPagenationOutputDto result = new UserPagenationOutputDto() { Rows = listDto, Total = total };
            return result;
        }

        public override async Task<UserDto> Create(CreateUserDto input)
        {
            CheckCreatePermission();

            var user = ObjectMapper.Map<User>(input);

            user.TenantId = AbpSession.TenantId;
            user.IsEmailConfirmed = true;

            await _userManager.InitializeOptionsAsync(AbpSession.TenantId);

            CheckErrors(await _userManager.CreateAsync(user, input.Password));

            if (input.RoleNames != null)
            {
                CheckErrors(await _userManager.SetRoles(user, input.RoleNames));
            }

            CurrentUnitOfWork.SaveChanges();

            return MapToEntityDto(user);
        }

        public override async Task<UserDto> Update(UserDto input)
        {
            CheckUpdatePermission();

            var user = await _userManager.GetUserByIdAsync(input.Id);

            MapToEntity(input, user);

            CheckErrors(await _userManager.UpdateAsync(user));

            if (input.RoleNames != null)
            {
                CheckErrors(await _userManager.SetRoles(user, input.RoleNames));
            }

            return await Get(input);
        }

        public override async Task Delete(EntityDto<long> input)
        {
            var user = await _userManager.GetUserByIdAsync(input.Id);
            await _userManager.DeleteAsync(user);
        }

        public async Task<ListResultDto<RoleDto>> GetRoles()
        {
            var roles = await _roleRepository.GetAllListAsync();
            return new ListResultDto<RoleDto>(ObjectMapper.Map<List<RoleDto>>(roles));
        }

        public async Task ChangeLanguage(ChangeUserLanguageDto input)
        {
            await SettingManager.ChangeSettingForUserAsync(
                AbpSession.ToUserIdentifier(),
                LocalizationSettingNames.DefaultLanguage,
                input.LanguageName
            );
        }

        protected override User MapToEntity(CreateUserDto createInput)
        {
            var user = ObjectMapper.Map<User>(createInput);
            user.SetNormalizedNames();
            return user;
        }

        protected override void MapToEntity(UserDto input, User user)
        {
            ObjectMapper.Map(input, user);
            user.SetNormalizedNames();
        }

        protected override UserDto MapToEntityDto(User user)
        {
            var roles = _roleManager.Roles.Where(r => user.Roles.Any(ur => ur.RoleId == r.Id)).Select(r => r.NormalizedName);
            var userDto = base.MapToEntityDto(user);
            userDto.RoleNames = roles.ToArray();
            return userDto;
        }

        protected override IQueryable<User> CreateFilteredQuery(PagedUserResultRequestDto input)
        {
            return Repository.GetAllIncluding(x => x.Roles)
                .WhereIf(!input.Keyword.IsNullOrWhiteSpace(), x => x.UserName.Contains(input.Keyword) || x.Name.Contains(input.Keyword) || x.EmailAddress.Contains(input.Keyword))
                .WhereIf(input.IsActive.HasValue, x => x.IsActive == input.IsActive);
        }

        protected override async Task<User> GetEntityByIdAsync(long id)
        {
            var user = await Repository.GetAllIncluding(x => x.Roles).FirstOrDefaultAsync(x => x.Id == id);

            if (user == null)
            {
                throw new EntityNotFoundException(typeof(User), id);
            }

            return user;
        }

        protected override IQueryable<User> ApplySorting(IQueryable<User> query, PagedUserResultRequestDto input)
        {
            return query.OrderBy(r => r.UserName);
        }

        protected virtual void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }

        /// <summary>
        /// 更改密码
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<bool> ChangePassword(ChangePasswordDto input)
        {
            if (_abpSession.UserId == null)
            {
                throw new UserFriendlyException("请登录后再进行修改密码");
            }
            long userId = _abpSession.UserId.Value;
            var user = await _userManager.GetUserByIdAsync(userId);
            var loginAsync = await _logInManager.LoginAsync(user.UserName, input.CurrentPassword, shouldLockout: false);
            if (loginAsync.Result != AbpLoginResultType.Success)
            {
                throw new UserFriendlyException("您的当前密码错误. 请重试或联系管理员以获取帮助");
            }
            if (!new Regex(AccountAppService.PasswordRegex).IsMatch(input.NewPassword))
            {
                throw new UserFriendlyException("密码至少为8位字符，包含大写、小写与数字");
            }
            user.Password = _passwordHasher.HashPassword(user, input.NewPassword);
            CurrentUnitOfWork.SaveChanges();
            return true;
        }

        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<bool> ResetPassword(ResetPasswordDto input)
        {
            if (_abpSession.UserId == null)
            {
                throw new UserFriendlyException("请在尝试重置密码之前登录.");
            }
            long currentUserId = _abpSession.UserId.Value;
            var currentUser = await _userManager.GetUserByIdAsync(currentUserId);
            var loginAsync = await _logInManager.LoginAsync(currentUser.UserName, input.AdminPassword, shouldLockout: false);
            if (loginAsync.Result != AbpLoginResultType.Success)
            {
                throw new UserFriendlyException("管理员密码错误，请重试");
            }
            if (currentUser.IsDeleted || !currentUser.IsActive)
            {
                return false;
            }
            var roles = await _userManager.GetRolesAsync(currentUser);
            if (!roles.Contains(StaticRoleNames.Tenants.Admin))
            {
                throw new UserFriendlyException("只有系统管理员可以重置密码");
            }

            var user = await _userManager.GetUserByIdAsync(input.UserId);
            if (user != null)
            {
                user.Password = _passwordHasher.HashPassword(user, input.NewPassword);
                CurrentUnitOfWork.SaveChanges();
            }

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<List<BaseUserDto>> GetUsersByDepartmentId(int id)
        {
            var dt = _helper.ExecuteQuery($@"select Id,Name from abpusers where Id in (select UserId from userdepartments where departmentId={id})");
            var list = new List<BaseUserDto>();
            foreach (DataRow item in dt.Rows)
            {
                list.Add(new BaseUserDto() { Id = Convert.ToInt32(item["id"]), UserName = item["Name"].ToString() });
            }
            return Task.FromResult(list);
        }
    }
}

