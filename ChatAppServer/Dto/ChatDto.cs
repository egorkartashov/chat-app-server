using System;
using System.Collections.Generic;

namespace ChatAppServer.Dto
{
	public class ChatDto
	{
		public Guid Id { get; set; }
		public IEnumerable<string> MembersEmails { get; set; }
		public string Name { get; set; }
		public DateTime? LastMessageTime { get; set; }
		public string? LastMessage { get; set; }
	}

	public enum ChatType
	{
		PersonalChat = 1,
		Chatroom = 2,
	}
}