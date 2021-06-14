using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ChatAppServer.ClientConnections;
using ChatAppServer.Dto;
using ChatAppServer.Services;
using Microsoft.AspNetCore.SignalR;

namespace ChatAppServer.ClientHubs
{
	public class ChatsHub : Hub
	{
		private readonly IChatsService _chatsService;
		private readonly IClientConnectionsCache _clientConnectionsCache;

		public ChatsHub(IChatsService chatsService, 
			IClientConnectionsCache clientConnectionsCache)
		{
			_chatsService = chatsService;
			_clientConnectionsCache = clientConnectionsCache;
		}
		
		public async Task<List<ChatDto>> GetChatsAsync()
		{
			Console.WriteLine($"GetChatsAsync, {Context.ConnectionId}");
			if (!_clientConnectionsCache.TryGetClientConnection(Context.ConnectionId, out var clientConnection))
				return new List<ChatDto>();

			var chatsDtos = await _chatsService.GetAvailableChatsAsync(clientConnection.User.Id);
			return chatsDtos;
		}
	}
}