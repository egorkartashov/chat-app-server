using System;
using System.Threading.Tasks;
using ChatAppServer.Dto;

namespace ChatAppServer.Services
{
	public interface IMessagesService
	{
		Task<bool> SaveMessageAsync(Guid senderId, Guid chatId, MessageDto messageDto);

		Task<bool> SavePersonalMessageAsync(Guid senderId, string receiverEmail, MessageDto messageDto);
	}
}