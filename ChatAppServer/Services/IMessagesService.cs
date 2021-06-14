using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ChatAppServer.Dto;

namespace ChatAppServer.Services
{
	public interface IMessagesService
	{
		Task<bool> SaveMessageToChatAsync(Guid senderId, Guid chatId, MessageDto messageDto);

		Task<bool> SavePersonalMessageAsync(Guid senderId, string receiverEmail, MessageDto messageDto);

		Task<List<MessageDto>> GetChatMessagesAsync(Guid chatId);
	}
}