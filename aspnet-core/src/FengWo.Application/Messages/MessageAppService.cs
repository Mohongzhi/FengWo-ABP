using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.ObjectMapping;
using Abp.Runtime.Session;
using Abp.UI;
using FengWo.Messages.Dto;

namespace FengWo.Messages
{
    /// <summary>
    /// 消息推送服务
    /// </summary>
    public class MessageAppService : IMessageAppService
    {
        private readonly IRepository<Message> _MessageRepository;

        private readonly IAbpSession _abpSession;

        /// <summary>
        /// Reference to the object to object mapper.
        /// </summary>
        public readonly IObjectMapper _objectMapper;

        public MessageAppService(IRepository<Message> MessageRepository,
            IAbpSession abpSession, 
            IObjectMapper ObjectMapper)
        {
            _MessageRepository = MessageRepository;
            _abpSession = abpSession;
            _objectMapper = ObjectMapper;
        }

        public Task Broadcast(MessageDto message)
        {
            throw new NotImplementedException();
        }

        public IList<MessageDto> GetMyMessages()
        {
            if (_abpSession.UserId == null)
            {
                //throw new UserFriendlyException("Please log in before attemping to get messages.");
                return new List<MessageDto>();
            }

            var list = _MessageRepository.GetAllList(p => p.ReceiveUserId == _abpSession.UserId);

            return _objectMapper.Map<List<MessageDto>>(list);
        }

        public Task SendToArr(IList<MessageDto> messages)
        {
            throw new NotImplementedException();
        }

        public Task SendToSomeone(MessageDto message)
        {
            throw new NotImplementedException();
        }

        public void RemoveMessage(string url)
        {
            if (_abpSession.UserId != null)
            {
                _MessageRepository.DeleteAsync(p => p.ReceiveUserId == _abpSession.UserId && p.Url == url);
            }
        }
    }
}
