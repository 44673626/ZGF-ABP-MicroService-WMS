using COM.CAP.EtoShare;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace BaseService.Systems.UserManagement
{
    public interface ICAPUserAppService : IApplicationService
    {
        Task CreateUserAsync(Cap_AddOrModifyUserEto eto);

        Task UpdateUserAsync(Cap_AddOrModifyUserEto eto);

        Task DeleteUserAsync(Cap_RemoveUserEto eto);

        Task ResetPasswordAsync(Cap_ChangPasswordEto eto);

        Task CreateRoleAsync(Cap_AddRoleEto eto);

        Task UpdateRoleAsync(Cap_ModdifyRoleEto eto);

        Task DeteleRoleAsync(Cap_RemoveRoleEto eto);

    }
}
