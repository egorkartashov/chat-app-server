﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ChatAppServer.Auth;
using ChatAppServer.ClientConnections;
using ChatAppServer.Dto;
using ChatAppServer.Exceptions;
using ChatAppServer.Models;
using ChatAppServer.Services;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;

namespace ChatAppServer.ClientHubs
{
	[EnableCors]
	public class ClientHub : Hub<IChatClient>
	{
		private readonly IClientConnectionsCache _clientConnectionsCache;
		private readonly IUsersService _usersService;
		private readonly IChatsService _chatsService;
		private readonly IMessagesService _messagesService;

		public ClientHub(IClientConnectionsCache clientConnectionsCache,
			IUsersService usersService, IChatsService chatsService,
			IMessagesService messagesService)
		{
			_clientConnectionsCache = clientConnectionsCache;
			_usersService = usersService;
			_chatsService = chatsService;
			_messagesService = messagesService;
		}

		public async Task SendPersonalMessageAsync(string receiverEmail, MessageDto messageDto)
		{
			Console.WriteLine($"SendMessageAsync {Context.ConnectionId}, {JsonConvert.SerializeObject(messageDto)}");
			if (_clientConnectionsCache.TryGetClientConnection(Context.ConnectionId, out var clientConnection))
			{
				var userId = clientConnection.User.Id;
				try
				{
					var messageSaved = await _messagesService.SavePersonalMessageAsync(userId, receiverEmail, messageDto);
					if (messageSaved)
					{
						// TODO send new message to other members of chatroom
					}
				}
				catch (UserNotFoundException e)
				{
					await Clients.Client(Context.ConnectionId).NotifyMessageReceivedAsync($"User not found: {e.Email}");
				}
			}
		}

		public async Task<User> GetUserByEmailAsync(string email)
		{
			Console.WriteLine($"GetUserByEmailAsync, email={email}");
			var user = await _usersService.GetUserByEmail(email);
			return user;
		}
		
		public async Task<List<ChatDto>> GetChatsAsync()
		{
			Console.WriteLine($"GetChatsAsync, {Context.ConnectionId}");
			if (!_clientConnectionsCache.TryGetClientConnection(Context.ConnectionId, out var clientConnection))
				return new List<ChatDto>();

			var chatsDtos = await _chatsService.GetAvailableChatsAsync(clientConnection.User.Id);
			return chatsDtos;
		}

		public override async Task OnConnectedAsync()
		{
			var httpContext = Context.GetHttpContext();
			var accessTokenBearer = httpContext.Request.Query["access_token"].ToString();
			var connectionId = Context.ConnectionId;
			
			try
			{
				if (TryExtractAccessToken(accessTokenBearer, out var accessToken))
				{
					var payload = await GoogleJsonWebSignature.ValidateAsync(accessToken);
					var googleUserInfo = ConvertPayloadToGoogleUserInfo(payload);

					var user = await _usersService.AuthGoogleUserAsync(googleUserInfo);
					
					_clientConnectionsCache.SaveClientConnection(connectionId, new ClientConnection(accessToken, user));
				}

				Console.WriteLine($"Connected: {connectionId}");

				await base.OnConnectedAsync();
			}
			catch (InvalidJwtException invalidJwtException)
			{
				await Clients.Client(connectionId).NotifyMessageReceivedAsync("Invalid jwt");
				Console.WriteLine(invalidJwtException.Message + $" current time: {DateTime.UtcNow}");
				Context.Abort();
			}
			catch (Exception exception)
			{
				var exceptionJson = JsonConvert.SerializeObject(exception);
				Console.WriteLine(exceptionJson);
				await base.OnConnectedAsync();
			}
		}

		public override Task OnDisconnectedAsync(Exception exception)
		{
			Console.WriteLine($"Disconnected: {Context.ConnectionId}");
			return base.OnDisconnectedAsync(exception);
		}

		private static bool TryExtractAccessToken(string accessTokenBearer, out string accessToken)
		{
			accessToken = string.Empty;
			var accessTokenBearerParts = accessTokenBearer.Split();
			if (accessTokenBearerParts.Length != 2)
				return false;

			if (accessTokenBearerParts[0] != "Bearer")
				return false;

			accessToken = accessTokenBearerParts[1];
			return true;
		}

		private static GoogleUserInfo ConvertPayloadToGoogleUserInfo(GoogleJsonWebSignature.Payload payload)
		{
			var googleUserInfo = new GoogleUserInfo
			{
				Name = payload.Name,
				Email = payload.Email,
				ProfilePictureUrl = payload.Picture,
			};

			return googleUserInfo;
		}
	}
}