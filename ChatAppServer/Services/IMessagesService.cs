using System.Threading.Tasks;
using ChatAppServer.Dto;

namespace ChatAppServer.Services
{
	public interface IMessagesService
	{
		Task SaveDirectMessageAsync(DirectMessageDto directMessageDto);
	}
}