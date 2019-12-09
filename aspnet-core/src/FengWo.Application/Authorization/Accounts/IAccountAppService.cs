using System.Threading.Tasks;
using Abp.Application.Services;
using FengWo.Authorization.Accounts.Dto;

namespace FengWo.Authorization.Accounts
{
    public interface IAccountAppService : IApplicationService
    {
        Task<IsTenantAvailableOutput> IsTenantAvailable(IsTenantAvailableInput input);

        Task<RegisterOutput> Register(RegisterInput input);
    }
}
