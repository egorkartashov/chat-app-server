using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ChatAppServer.Dto;

namespace ChatAppServer.Services
{
	public interface IChatsService
	{
		Task<List<ChatDto>> GetAvailableChatsAsync(Guid userId);
	}
}