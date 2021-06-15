using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ChatAppServer.Dto;
using ChatAppServer.Models;

namespace ChatAppServer.Services
{
	public interface IMessagesService
	{
		Task<SaveMessageResult> SaveMessageToChatAsync(Guid senderId, Guid chatId, NewMessageDto messageDto);

		Task<SaveMessageResult> SavePersonalMessageAsync(Guid senderId, string receiverEmail, NewMessageDto messageDto);

		Task<List<MessageDto>> GetChatMessagesAsync(Guid chatId);
	}
}