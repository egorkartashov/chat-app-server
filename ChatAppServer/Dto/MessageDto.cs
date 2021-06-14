using System;
using Newtonsoft.Json;

namespace ChatAppServer.Dto
{
	public class MessageDto
	{
		[JsonProperty("sentTimeUtc")]
		public DateTime SentTimeUtc { get; set; }
		
		[JsonProperty("text")]
		public string Text { get; set; }
	}
}