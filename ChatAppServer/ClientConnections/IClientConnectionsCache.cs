namespace ChatAppServer.ClientConnections
{
	public interface IClientConnectionsCache
	{
		bool TryGetClientConnection(string connectionId, out ClientConnection clientConnection);
		
		void SaveClientConnection(string connectionId, ClientConnection clientConnection);
	}
}