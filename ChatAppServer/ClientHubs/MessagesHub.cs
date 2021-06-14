using System.Threading.Tasks;
using ChatAppServer.ClientConnections;
using ChatAppServer.Dto;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.SignalR;

namespace ChatAppServer.ClientHubs
{
	[EnableCors]
	public class MessagesHub : Hub
	{
		private readonly IClientConnectionsCache _clientConnectionsCache;

		public MessagesHub(IClientConnectionsCache clientConnectionsCache)
		{
			_clientConnectionsCache = clientConnectionsCache;
		}

		public async Task SendPersonalMessageAsync(MessageDto messageDto, string receiverEmail)
		{
			
		}
	}
}