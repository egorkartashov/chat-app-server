using ChatAppServer.Auth;

namespace ChatAppServer.ClientConnections
{
	public class ClientConnection
	{
		public ClientConnection(string accessToken, GoogleUserInfo googleUserInfo)
		{
			AccessToken = accessToken;
			GoogleUserInfo = googleUserInfo;
		}
		
		public string AccessToken { get;}
		public GoogleUserInfo GoogleUserInfo { get; }
	}
}