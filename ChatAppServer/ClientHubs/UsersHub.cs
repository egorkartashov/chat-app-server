using System;
using System.Threading.Tasks;
using ChatAppServer.Auth;
using ChatAppServer.ClientConnections;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.SignalR;

namespace ChatAppServer.ClientHubs
{
	[EnableCors]
	public class UsersHub : Hub
	{
		private readonly IClientConnectionsCache _clientConnectionsCache;

		public UsersHub(IClientConnectionsCache clientConnectionsCache)
		{
			_clientConnectionsCache = clientConnectionsCache;
		}

		public async Task SendMessageAsync(string message)
		{
			Console.WriteLine($"SendMessageAsync {Context.ConnectionId}");
			if (_clientConnectionsCache.TryGetClientConnection(Context.ConnectionId, out var clientConnection))
			{
				var accessToken = clientConnection.AccessToken;
				await Clients.Caller.SendAsync("NotifyMessageReceived", $"Received token: {accessToken}");
			};
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

					_clientConnectionsCache.SaveClientConnection(connectionId,
						new ClientConnection(accessToken, googleUserInfo));
				}

				Console.WriteLine($"Connected: {connectionId}");

				await base.OnConnectedAsync();
			}
			catch (InvalidJwtException invalidJwtException)
			{
				await Clients.Client(connectionId).SendAsync("NotifyMessageReceived", "Invalid jwt");
				Console.WriteLine(invalidJwtException.Message + $" current time: {DateTime.UtcNow}");
				Context.Abort();
			}
			catch (Exception)
			{
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