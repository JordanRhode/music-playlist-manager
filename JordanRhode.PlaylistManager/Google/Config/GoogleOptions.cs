using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace JordanRhode.PlaylistManager.Google.Config
{
	public class GoogleOptions
	{
		[Required]
		public required string Scope { get; set; }

		[Required]
		public required double JwtExpirationMinutes { get; set; }
	}
}
