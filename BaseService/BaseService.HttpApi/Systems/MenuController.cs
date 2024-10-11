using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BaseService.Controllers;
using BaseService.Systems.MenuManagement;
using BaseService.Systems.MenuManagement.Dto;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;

namespace BaseService.HttpApi.Systems
{
    [Area("base")]
    [Route("api/base/menu")]
    //[ApiExplorerSettings(GroupName = "v1")]
    public class MenuController : BaseServiceController, IMenuAppService
    {
        private readonly IMenuAppService _menuAppService;

        public MenuController(IMenuAppService menuAppService)
        {
            _menuAppService = menuAppService;
        }

        [HttpPost]
        public Task<MenuDto> Create(CreateOrUpdateMenuDto input)
        {
            return _menuAppService.Create(input);
        }

        [HttpPost]
        [Route("delete")]
        public Task Delete(List<Guid> ids)
        {
            return _menuAppService.Delete(ids);
        }

        [HttpGet]
        [Route("{id}")]
        public Task<MenuDto> Get(Guid id)
        {
            return _menuAppService.Get(id);
        }

        [HttpGet]
        [Route("all")]
        public Task<ListResultDto<MenuDto>> GetAll(GetMenuInputDto input)
        {
            return _menuAppService.GetAll(input);
        }

        /// <summary>
        /// 用于区分接入的不同项目
        /// </summary>
        /// <param name="input"></param>
        /// <param name="AppId">项目ID 保持唯一，比如项目号</param>
        /// <returns></returns>
        [HttpGet]
        [Route("all/appid")]
        public Task<ListResultDto<MenuDto>> GetAll(GetMenuInputDto input, string AppId)
        {
            return _menuAppService.GetAll(input, AppId);
        }

        [HttpGet]
        [Route("loadMenus")]
        public Task<ListResultDto<MenuDto>> LoadAll(Guid? id)
        {
            return _menuAppService.LoadAll(id);
        }

        [HttpGet]
        [Route("loadMenus/appid")]
        public Task<ListResultDto<MenuDto>> LoadAll(Guid? id, string AppId)
        {
            return _menuAppService.LoadAll(id, AppId);
        }

        [HttpPut]
        [Route("{id}")]
        public Task<MenuDto> Update(Guid id, CreateOrUpdateMenuDto input)
        {
            return _menuAppService.Update(id, input);
        }
    }
}
