using Google.Apis.Auth.OAuth2;
using Microsoft.Extensions.Configuration;

namespace ChatAppServer.Auth
{
	public class GoogleClientSecretsProvider : IGoogleClientSecretsProvider
	{
		private readonly ClientSecrets _clientSecrets;
		
		public GoogleClientSecretsProvider(IConfiguration configuration)
		{
			var googleAuthenticationSettings = configuration.GetSection("Authentication:Google");
			var clientId = googleAuthenticationSettings["ClientId"];
			var clientSecret = googleAuthenticationSettings["ClientSecret"];
			_clientSecrets = new ClientSecrets()
			{
				ClientId = clientId,
				ClientSecret = clientSecret,
			};
		}

		public ClientSecrets GetClientSecrets() => _clientSecrets;
	}
}