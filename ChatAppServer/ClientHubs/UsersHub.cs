using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.SignalR;

namespace ChatAppServer.ClientHubs
{
	[EnableCors()]
	public class UsersHub : Hub
	{
		public async Task SendMessageAsync(string message)
		{
			await Clients.Caller.SendAsync("NotifyMessageReceived", $"Received message: {message}");
		}
	
		public override Task OnConnectedAsync()
		{
			// TODO: Authorization
			Console.WriteLine("New client connected");
			
			return base.OnConnectedAsync();
		}
	}
}