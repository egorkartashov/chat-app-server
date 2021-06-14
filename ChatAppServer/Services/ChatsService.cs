using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatAppServer.DataAccess;
using ChatAppServer.Dto;
using ChatAppServer.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace ChatAppServer.Services
{
	public class ChatsService : IChatsService
	{
		private readonly ChatDbContext _chatDbContext;

		public ChatsService(ChatDbContext chatDbContext)
		{
			_chatDbContext = chatDbContext;
		}
		
		public async Task<List<ChatDto>> GetAvailableChatsAsync(Guid userId)
		{
			var user = await _chatDbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
			
			if (user == null)
				throw new UserNotFoundException(userId);

			var chatroomDtos = await _chatDbContext.Chatrooms
				                   .Where(chatroom => chatroom.Members.Contains(user))
				                   .Select(chatroom => new ChatDto
				                   {
					                   Id = chatroom.Id,
					                   Name = chatroom.Name,
					                   MembersEmails = chatroom.Members.Select(member => member.Email),
					                   LastMessage = chatroom.Messages.OrderByDescending(m => m.SentTime).First().Text,
					                   LastMessageTime = chatroom.Messages.OrderByDescending(m => m.SentTime).First().SentTime,
				                   })
				                   .ToListAsync();

			return chatroomDtos;
		}
	}
}