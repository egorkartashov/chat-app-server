using Google.Apis.Auth.OAuth2;

namespace ChatAppServer.Auth
{
	public interface IGoogleClientSecretsProvider
	{
		ClientSecrets GetClientSecrets();
	}
}