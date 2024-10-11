using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;

namespace HangFireJob.Samples;

[Area(HangFireJobRemoteServiceConsts.ModuleName)]
[RemoteService(Name = HangFireJobRemoteServiceConsts.RemoteServiceName)]
[Route("api/HangFireJob/sample")]
[ApiExplorerSettings(IgnoreApi = true)]
public class SampleController : HangFireJobController, ISampleAppService
{
    private readonly ISampleAppService _sampleAppService;

    public SampleController(ISampleAppService sampleAppService)
    {
        _sampleAppService = sampleAppService;
    }

    [HttpGet]
    public async Task<SampleDto> GetAsync()
    {
        return await _sampleAppService.GetAsync();
    }

    [HttpGet]
    [Route("authorized")]
    [Authorize]
    public async Task<SampleDto> GetAuthorizedAsync()
    {
        return await _sampleAppService.GetAsync();
    }
}
