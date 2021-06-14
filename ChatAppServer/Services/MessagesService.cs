using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ChatAppServer.DataAccess;
using ChatAppServer.DataAccess.Entities;
using ChatAppServer.Dto;
using ChatAppServer.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace ChatAppServer.Services
{
	public class MessagesService : IMessagesService
	{
		private readonly ChatDbContext _chatDbContext;

		public MessagesService(ChatDbContext chatDbContext)
		{
			_chatDbContext = chatDbContext;
		}
		
		public async Task<bool> SaveMessageAsync(Guid senderId, Guid chatId, MessageDto messageDto)
		{
			try
			{
				var chatroom = await _chatDbContext.Chatrooms.FindAsync(chatId);
				if (chatroom == null)
					throw new ChatroomNotFoundException(chatId);

				var sender = await _chatDbContext.Users.FindAsync(senderId);
				if (sender == null)
					throw new UserNotFoundException(senderId);

				var chatroomMessageEntity = new ChatroomMessageEntity()
				{
					Chatroom = chatroom,
					Sender = sender,
					SentTime = messageDto.SentTimeUtc,
					Text = messageDto.Text,
				};

				await _chatDbContext.ChatroomMessages.AddAsync(chatroomMessageEntity);
				await _chatDbContext.SaveChangesAsync();

				return true;
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				return false;
			}
		}

		public async Task<bool> SavePersonalMessageAsync(Guid senderId, string receiverEmail, MessageDto messageDto)
		{
			try
			{
				var receiver = await _chatDbContext.Users.FirstOrDefaultAsync(user => user.Email == receiverEmail);
				if (receiver == null)
					throw new UserNotFoundException(receiverEmail);

				var sender = await _chatDbContext.Users.FindAsync(senderId);
				if (sender == null)
					throw new UserNotFoundException(senderId);

				var chatroomEntity = new ChatroomEntity
				{
					Members = new List<UserEntity> { sender, receiver },
					Name = $"{sender.Name} - {receiver.Name} chat",
					OwnerId = null,
				};
				
				var chatroomMessageEntity = new ChatroomMessageEntity
				{
					Chatroom = chatroomEntity,
					Sender = sender,
					SentTime = messageDto.SentTimeUtc,
					Text = messageDto.Text,
				};

				await _chatDbContext.Chatrooms.AddAsync(chatroomEntity);
				await _chatDbContext.ChatroomMessages.AddAsync(chatroomMessageEntity);
				await _chatDbContext.SaveChangesAsync();

				return true;
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				return false;
			}
		}
	}
}