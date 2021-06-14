using System.Collections.Generic;
using Newtonsoft.Json;

namespace ChatAppServer.Dto
{
	public class ChatroomDto
	{
		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("members")]
		public IEnumerable<string> Members { get; set; }
	}
}