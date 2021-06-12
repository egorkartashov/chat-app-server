using ChatAppServer.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.SignalR;

namespace ChatAppServer.ClientHubs
{
	[EnableCors]
	public class MessagesHub : Hub
	{
		
		private readonly IMessagesService _messagesService;

		public MessagesHub(IMessagesService messagesService)
		{
			_messagesService = messagesService;
		}
	}
}