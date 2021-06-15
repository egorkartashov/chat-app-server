using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatAppServer.DataAccess;
using ChatAppServer.DataAccess.Entities;
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
			
			var chatroomMiddleDtos = await _chatDbContext.Chatrooms
				                   .Include(chatroom => chatroom.Messages)
				                   .Include(chatroom => chatroom.Members)
				                   .Where(chatroom => chatroom.Members.Contains(user))
				                   .Select(chatroom => new ChatroomMiddleDto()
				                   {
					                   Id = chatroom.Id,
					                   Name = chatroom.Name,
					                   MembersEmails = chatroom.Members.Select(member => member.Email),
					                   LastMessage = chatroom.Messages.OrderByDescending(m => m.SentTime).FirstOrDefault(),
				                   })
				                   .ToListAsync();

			var chatroomDtos = chatroomMiddleDtos
				.Select(almostDto => new ChatDto
				{
					Id = almostDto.Id,
					Name = almostDto.Name,
					MembersEmails = almostDto.MembersEmails,
					LastMessage = almostDto.LastMessage?.Text,
					LastMessageTime = almostDto.LastMessage != null 
						? DateTime.SpecifyKind(almostDto.LastMessage.SentTime, DateTimeKind.Utc) 
						: null,
				})
				.ToList();

			return chatroomDtos;
		}

		public async Task<bool> CreateChatroomAsync(Guid creatorId, ChatroomDto chatroomDto)
		{
			var emailsHashset = new HashSet<string>(chatroomDto.Members);
			var members = await _chatDbContext.Users
				              .Where(user => emailsHashset.Contains(user.Email))
				              .ToListAsync();

			var owner = await _chatDbContext.Users.FindAsync(creatorId);
			if (owner == null)
				throw new UserNotFoundException(creatorId, "Creator of chatroom was not found");

			if (members.All(member => member.Id != creatorId))
				members.Add(owner);

			var chatroom = new ChatroomEntity
			{
				Name = chatroomDto.Name,
				Members = members,
				Owner = owner,
			};

			await _chatDbContext.Chatrooms.AddAsync(chatroom);
			await _chatDbContext.SaveChangesAsync();

			return true;
		}

		private class ChatroomMiddleDto
		{
			public Guid Id { get; set; }

			public string Name { get; set; }

			public IEnumerable<string> MembersEmails { get; set; }

			public ChatroomMessageEntity LastMessage { get; set; }
		}
	}
}