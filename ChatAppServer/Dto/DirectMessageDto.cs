using System;

namespace ChatAppServer.Dto
{
	public class DirectMessageDto
	{
		public DateTime SentTimeUtc { get; set; }
		public string Text { get; set; }
		public Guid ReceiverId { get; set; }
	}
}