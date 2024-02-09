using System.ComponentModel.DataAnnotations;

namespace JordanRhode.PlaylistManager.Google.Config;

public class GoogleOptions
{
	[Required]
	public required string Scope { get; set; }

	[Required]
	public required string GrantType { get; set; }

	[Required]
	public required double JwtExpirationMinutes { get; set; }
}
