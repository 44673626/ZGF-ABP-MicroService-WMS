using AutoMapper.Internal.Mappers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Data;
using Volo.Abp.Guids;
using Volo.Abp.Identity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.PermissionManagement;
using Volo.Abp.Users;
using Volo.Abp;
using BaseService.BaseData.Secret;
using Volo.Abp.ObjectExtending;
using Microsoft.AspNetCore.Identity;
using COM.CAP.EtoShare;
using IdentityUser = Volo.Abp.Identity.IdentityUser;
using IdentityRole = Volo.Abp.Identity.IdentityRole;

namespace BaseService.Systems.UserManagement
{
    public class CAPUserAppService : ApplicationService, ICAPUserAppService
    {
        //读取配置文件
        private readonly IHostingEnvironment _env;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IDataFilter _dataFilter;
        private readonly IConfiguration _appConfiguration;

        //IDbContextProvider<IdentityUser>

        /// <summary>
        /// 用户管理
        /// </summary>
        protected IdentityUserManager UserManager { get; }
        /// <summary>
        /// 用户管理仓储
        /// </summary>
        protected IIdentityUserRepository UserRepository { get; }
        /// <summary>
        /// 角色仓储
        /// </summary>
        public IIdentityRoleRepository RoleRepository { get; }


        protected ICurrentUser CurrentUsers { get; }

        private readonly IGuidGenerator _guidGenerator;

        private readonly IdentityRoleManager _roleManager;

        /// <summary>
        /// 权限列表
        /// </summary>
        protected IPermissionGrantRepository PermissionGrantRepository { get; }

        public CAPUserAppService(
            IdentityUserManager userManager,
            IIdentityUserRepository userRepository,
            IIdentityRoleRepository roleRepository,
            ICurrentUser currentUser,
            IGuidGenerator guidGenerator,
            IdentityRoleManager roleManager,
            IHttpClientFactory httpClientFactory,
            IHostingEnvironment env,
            IPermissionGrantRepository permissionGrantRepository,
            IConfiguration appConfiguration,
            IDataFilter dataFilter)
        {
            UserManager = userManager;
            UserRepository = userRepository;
            RoleRepository = roleRepository;
            CurrentUsers = currentUser;
            _guidGenerator = guidGenerator;
            PermissionGrantRepository = permissionGrantRepository;
            _roleManager = roleManager;
            //读取配置文件
            _env = env;
            _httpClientFactory = httpClientFactory;
            _dataFilter = dataFilter;
            _appConfiguration = appConfiguration;
        }

        /// <summary>
        /// 创建用户信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task CreateUserAsync(Cap_AddOrModifyUserEto eto)
        {
            if (string.IsNullOrEmpty(eto.RoleName)) throw new BusinessException(message: "角色名信息不能为空！");
            var getRoleNames = eto.RoleName.Split(",");
            string errRoleNames = string.Empty;
            foreach (var roleName in getRoleNames)
            {
                var role = (await RoleRepository.GetListAsync()).Where(p => p.Name == roleName).Distinct().FirstOrDefault();
                if (role == null)
                {
                    errRoleNames += roleName + ",";
                }
            }
            if (!string.IsNullOrEmpty(errRoleNames))
            {
                throw new BusinessException(message: $"角色名：{errRoleNames.TrimEnd(',')}，在角色表中不存在,请先同步角色信息！");
            }

            IdentityUserCreateDto input = new IdentityUserCreateDto();
            input.UserName = eto.LoginName;//登录名
            input.Name = eto.UserName;
            input.Email = string.IsNullOrEmpty(eto.Email) ? $"{_guidGenerator.Create()}@abp.io" : eto.Email;
            input.Password = eto.Password;
            input.PhoneNumber = eto.Mobile;
            if (eto.Enable == "0")
            {
                input.LockoutEnabled = false;
            }
            else
            {
                input.LockoutEnabled = true;
            }
            var list = (await UserRepository.GetListAsync()).Select(p => new { p.Id, p.UserName });//查询指定列
            //当前系统用户列表中是否存在要添加的登录用户
            var getuser = list.Where(p => p.UserName == input.UserName).FirstOrDefault();
            if (getuser == null)
            {
                var user = new IdentityUser(
                    GuidGenerator.Create(),
                    input.UserName,
                    input.Email,
                    CurrentTenant.Id
                );

                input.MapExtraPropertiesTo(user);

                (await UserManager.CreateAsync(user, input.Password)).CheckErrors();
                await UpdateAsync(user, input);
                await ChangeUserActiveStatus(user.Id, input.LockoutEnabled);//是否锁定用户
                var dto = ObjectMapper.Map<IdentityUser, IdentityUserDto>(user);

                await CurrentUnitOfWork.SaveChangesAsync();
                //往用户角色表中添加数据，维护用户和角色关系信息
                await UserManager.SetRolesAsync(user, getRoleNames.ToList());//方法内有删除操作
            }
            //return dto;
        }

        /// <summary>
        /// 修改用户信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task UpdateUserAsync(Cap_AddOrModifyUserEto eto)
        {
            if (string.IsNullOrEmpty(eto.RoleName)) throw new BusinessException(message:"角色名信息不能为空！");
            var getRoleNames = eto.RoleName.Split(",");
            string errRoleNames = string.Empty;
            foreach (var roleName in getRoleNames)
            {
                var role = (await RoleRepository.GetListAsync()).Where(p => p.Name == roleName).Distinct().FirstOrDefault();
                if (role == null)
                {
                    errRoleNames += roleName + ",";
                }
            }
            if (!string.IsNullOrEmpty(errRoleNames))
            {
                throw new BusinessException(message: $"角色名：{errRoleNames.TrimEnd(',')}，在角色表中不存在,请先同步角色信息！");
            }
            IdentityUserUpdateDto input = new IdentityUserUpdateDto();
            input.UserName = eto.LoginName;
            input.Name = eto.UserName;
            input.Email = string.IsNullOrEmpty(eto.Email) ? $"{_guidGenerator.Create()}@abp.io" : eto.Email;
            input.Password = eto.Password;
            input.PhoneNumber = eto.Mobile;
            if (eto.Enable == "0")
            {
                input.LockoutEnabled = false;//解锁
            }
            else
            {
                input.LockoutEnabled = true;//锁定
            }
            var userCheck = (await UserRepository.GetListAsync()).Where(p => p.UserName == input.UserName).FirstOrDefault();
            if (userCheck != null)
            {
                var user = await UserManager.GetByIdAsync(userCheck.Id).ConfigureAwait(false);
                input.ConcurrencyStamp = user.ConcurrencyStamp;//乐观并发控制

                (await UserManager.SetUserNameAsync(user, input.UserName)).CheckErrors();

                await UpdateAsync(user, input);
                input.MapExtraPropertiesTo(user);

                (await UserManager.UpdateAsync(user)).CheckErrors();

                if (!input.Password.IsNullOrEmpty())
                {
                    (await UserManager.RemovePasswordAsync(user)).CheckErrors();
                    (await UserManager.AddPasswordAsync(user, input.Password)).CheckErrors();
                }
                await ChangeUserActiveStatus(user.Id, input.LockoutEnabled);//是否锁定用户
                var dto = ObjectMapper.Map<IdentityUser, IdentityUserDto>(user);

                await CurrentUnitOfWork.SaveChangesAsync();

                //往用户角色表中添加数据，维护用户和角色关系信息
                await UserManager.SetRolesAsync(user, getRoleNames.ToList());//方法内有删除操作

            }
            else
            {
                if (eto.Enable == "0")
                {
                    await CreateUserAsync(eto);
                }
            }
        }



        public async Task ChangeUserActiveStatus(Guid userId, bool isActive)
        {
            var user = await UserManager.FindByIdAsync(userId.ToString());
            if (user != null)
            {
                // 更改用户的激活状态
                await UserManager.SetLockoutEnabledAsync(user, !isActive);

                if (!isActive)
                {
                    // 设置锁定结束时间为永久
                    await UserManager.SetLockoutEndDateAsync(user, DateTimeOffset.MaxValue);
                }
                else
                {
                    // 解除锁定
                    await UserManager.SetLockoutEndDateAsync(user, DateTimeOffset.UtcNow);
                }
            }
        }




        protected virtual async Task UpdateAsync(IdentityUser user, IdentityUserCreateOrUpdateDtoBase input)
        {
            if (!string.Equals(user.Email, input.Email, StringComparison.InvariantCultureIgnoreCase))
            {
                (await UserManager.SetEmailAsync(user, input.Email)).CheckErrors();
            }

            if (!string.Equals(user.PhoneNumber, input.PhoneNumber, StringComparison.InvariantCultureIgnoreCase))
            {
                (await UserManager.SetPhoneNumberAsync(user, input.PhoneNumber)).CheckErrors();
            }

           (await UserManager.SetLockoutEnabledAsync(user, input.LockoutEnabled)).CheckErrors();

            user.Name = input.Name;
            user.Surname = input.Surname;


            if (input.RoleNames != null)
            {
                (await UserManager.SetRolesAsync(user, input.RoleNames)).CheckErrors();
            }
        }

        /// <summary>
        /// 根据登录用户名进行删除
        /// </summary>
        /// <param name="loginName">以逗号分隔的字符串</param>
        /// <returns></returns>
        public async Task DeleteUserAsync(Cap_RemoveUserEto eto)
        {
            var loginNames = eto.LoginName;
            if (!string.IsNullOrEmpty(loginNames))
            {
                foreach (var loginName in loginNames.Split(','))
                {
                    var userCheck = (await UserRepository.GetListAsync()).Where(p => p.UserName == loginName).FirstOrDefault();
                    if (userCheck != null)
                    {
                        var user = await UserManager.GetByIdAsync(userCheck.Id);
                        user.IsDeleted = true;
                        user.DeletionTime = DateTime.Now;
                        var dto = ObjectMapper.Map<IdentityUser, IdentityUserDto>(user);
                        await CurrentUnitOfWork.SaveChangesAsync();
                    }
                }
            }

        }

        /// <summary>
        /// 修改用户密码
        /// </summary>
        /// <param name="loginName">登录名</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        public async Task ResetPasswordAsync(Cap_ChangPasswordEto eto)
        {
            string loginName = eto.LoginName;
            string password = eto.Password.DecryptDES(_appConfiguration["RabbitMQ:SecretUser"]);//解密 
            var userCheck = (await UserRepository.GetListAsync()).Where(p => p.UserName == loginName).FirstOrDefault();
            if (userCheck != null)
            {
                var user = await UserManager.FindByIdAsync(userCheck.Id.ToString());
                await UserManager.RemovePasswordAsync(user);
                if (user.PasswordHash == null)
                {
                    (await UserManager.AddPasswordAsync(user, password)).CheckErrors();
                }
                var dto = ObjectMapper.Map<IdentityUser, IdentityUserDto>(user);
            }
        }

        /// <summary>
        /// 添加角色信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task CreateRoleAsync(Cap_AddRoleEto eto)
        {
            var roleCheck = (await RoleRepository.GetListAsync()).Where(p => p.Name == eto.RoleName).FirstOrDefault();
            if (roleCheck == null)
            {
                IdentityRole role = new IdentityRole(GuidGenerator.Create(), eto.RoleName);
                await RoleRepository.InsertAsync(role, autoSave: true).ConfigureAwait(false);
            }
            else
            {
                throw new BusinessException("01", $"角色名：{eto.RoleName}，在角色表中已经存在.");
            }
        }

        /// <summary>
        /// 修改角色信息
        /// </summary>
        /// <param name="orginalRoleName"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task UpdateRoleAsync(Cap_ModdifyRoleEto eto)
        {
            string orginalRoleName = eto.OriginalRoleName;
            var roleCheck = (await RoleRepository.GetListAsync()).Where(p => p.Name == orginalRoleName).FirstOrDefault();
            if (roleCheck != null)
            {
                var roleId = roleCheck.Id;
                var role = await _roleManager.GetByIdAsync(roleId);
                await _roleManager.SetRoleNameAsync(role, eto.ChangedRoleName);
            }
            else { throw new BusinessException(message: $"角色名：{orginalRoleName}，在角色表中已经存在."); }
        }

        /// <summary>
        /// 根据角色名进行删除
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns></returns>
        public async Task DeteleRoleAsync(Cap_RemoveRoleEto eto)
        {
            var roleNames = eto.RoleName;
            if (!string.IsNullOrEmpty(roleNames))
            {
                foreach (var roleName in roleNames.Split(','))
                {
                    var roleCheck = (await RoleRepository.GetListAsync()).Where(p => p.Name == roleName).FirstOrDefault();
                    if (roleCheck != null)
                    {
                        await _roleManager.DeleteAsync(roleCheck);
                    }
                }
            }
        }

    }
}
