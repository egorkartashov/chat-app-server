using System;
using Newtonsoft.Json;

namespace ChatAppServer.Dto
{
	public class MessageDto
	{
		public DateTime SentTimeUtc { get; set; }
		
		public string Text { get; set; }

		public Guid SenderId { get; set; }

		public string SenderEmail { get; set; }

		public string SenderName { get; set; }
	}
}