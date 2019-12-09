using Abp.Application.Services;
using FengWo.Messages.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FengWo.Messages
{
    public interface IMessageAppService : IApplicationService
    {
        Task SendToSomeone(MessageDto message);

        Task Broadcast(MessageDto message);

        Task SendToArr(IList<MessageDto> messages);

        IList<MessageDto> GetMyMessages();
    }
}
