using System.Threading.Tasks;

namespace ChatAppServer.ClientHubs
{
	public interface IChatClient
	{
		Task NotifyMessageReceivedAsync(string message);
	}
}