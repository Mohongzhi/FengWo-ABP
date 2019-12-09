using System.Threading.Tasks;
using Abp.Application.Services;
using FengWo.Sessions.Dto;

namespace FengWo.Sessions
{
    public interface ISessionAppService : IApplicationService
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations();
    }
}
