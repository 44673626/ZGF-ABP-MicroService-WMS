using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace WMS.BaseService.Samples;

public interface ISampleAppService : IApplicationService
{
    Task<SampleDto> GetAsync();

    Task<SampleDto> GetAuthorizedAsync();
}
