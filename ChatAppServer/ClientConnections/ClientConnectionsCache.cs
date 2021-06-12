using System;
using System.Collections.Generic;

namespace ChatAppServer.ClientConnections
{
	public class ClientConnectionsCache : IClientConnectionsCache
	{
		private readonly Dictionary<string, ClientConnection> _clientIdToClientConnectionMap = new Dictionary<string, ClientConnection>();

		public bool TryGetClientConnection(string connectionId, out ClientConnection clientConnection)
		{
			var clientConnectionFound = _clientIdToClientConnectionMap.TryGetValue(connectionId, out clientConnection);
			return clientConnectionFound;
		}

		public void SaveClientConnection(string connectionId, ClientConnection clientConnection)
		{
			if (connectionId == null) throw new ArgumentNullException(nameof(connectionId));
			if (clientConnection == null) throw new ArgumentNullException(nameof(clientConnection));
			
			_clientIdToClientConnectionMap[connectionId] = clientConnection;
		}
	}
}