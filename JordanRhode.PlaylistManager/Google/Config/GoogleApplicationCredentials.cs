using System.ComponentModel.DataAnnotations;

namespace JordanRhode.PlaylistManager.Google.Config
{
	public class GoogleApplicationCredentials
	{
		[Required]
		[ConfigurationKeyName("type")]
		public required string Type { get; set; }

		[Required]
		[ConfigurationKeyName("project_id")]
		public required string ProjectId { get; set; }

		[Required]
		[ConfigurationKeyName("private_key_id")]
		public required string PrivateKeyId { get; set; }

		[Required]
		[ConfigurationKeyName("private_key")]
		public required string PrivateKey { get; set; }

		[Required]
		[ConfigurationKeyName("client_email")]
		public required string ClientEmail { get; set; }

		[Required]
		[ConfigurationKeyName("client_id")]
		public required string ClientId { get; set; }

		[Required]
		[ConfigurationKeyName("auth_uri")]
		public required Uri AuthUri { get; set; }

		[Required]
		[ConfigurationKeyName("token_uri")]
		public required Uri TokenUri { get; set; }

		[Required]
		[ConfigurationKeyName("auth_provider_x509_cert_url")]
		public required Uri AuthProviderCertUrl { get; set; }

		[Required]
		[ConfigurationKeyName("client_x509_cert_url")]
		public required Uri ClientCertUrl { get; set; }

		[Required]
		[ConfigurationKeyName("universe_domain")]
		public required string UniverseDomain { get; set; }
	}
}
