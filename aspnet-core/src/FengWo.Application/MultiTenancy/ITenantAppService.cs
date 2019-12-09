using Abp.Application.Services;
using Abp.Application.Services.Dto;
using FengWo.MultiTenancy.Dto;

namespace FengWo.MultiTenancy
{
    public interface ITenantAppService : IAsyncCrudAppService<TenantDto, int, PagedTenantResultRequestDto, CreateTenantDto, TenantDto>
    {
    }
}

