using JordanRhode.PlaylistManager.Google.Config;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace JordanRhode.PlaylistManager.Google;

public static class GoogleUtilities
{
	public static string GenerateAuthenticationToken(GoogleApplicationCredentials credentials, GoogleOptions options)
	{
		ArgumentNullException.ThrowIfNull(credentials, nameof(credentials));
		ArgumentNullException.ThrowIfNull(options, nameof(options));

		var signingCredentials = GenerateRsaSigningCredentials(credentials.PrivateKey, credentials.PrivateKeyId);

		var tokenDescriptor = new SecurityTokenDescriptor
		{
			Issuer = credentials.ClientEmail,
			Subject = new ClaimsIdentity(new[]
			{
				new Claim("scope", options.Scope)
			}),
			Audience = credentials.TokenUri.ToString(),
			Expires = DateTime.UtcNow.AddMinutes(options.JwtExpirationMinutes),
			SigningCredentials = signingCredentials
		};
		var tokenHandler = new JwtSecurityTokenHandler();
		var securityToken = tokenHandler.CreateToken(tokenDescriptor);
		return tokenHandler.WriteToken(securityToken);
	}

	private static SigningCredentials GenerateRsaSigningCredentials(string privateKey, string privateKeyId)
	{
		using var rsa = RSA.Create();
		var cleanedKey = privateKey
			.Replace("-----BEGIN PRIVATE KEY-----", string.Empty, StringComparison.InvariantCultureIgnoreCase)
			.Replace("-----END PRIVATE KEY-----", string.Empty, StringComparison.InvariantCultureIgnoreCase);
		cleanedKey = cleanedKey.Replace(Environment.NewLine, string.Empty, StringComparison.InvariantCultureIgnoreCase);
		var privateKeyBytes = Convert.FromBase64String(privateKey);
		rsa.ImportPkcs8PrivateKey(privateKeyBytes, out int _);

		return new SigningCredentials(
			new RsaSecurityKey(rsa)
			{
				KeyId = privateKeyId
			},
			SecurityAlgorithms.RsaSha256);
	}
}
