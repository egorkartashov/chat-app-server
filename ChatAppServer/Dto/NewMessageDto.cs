using System;

namespace ChatAppServer.Dto
{
	public class NewMessageDto
	{
		public string Text { get; set; }

		public DateTime SentTimeUtc { get; set; }
	}
}