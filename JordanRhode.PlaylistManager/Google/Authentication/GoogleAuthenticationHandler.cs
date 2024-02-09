
using System.Net.Http.Headers;
using JordanRhode.PlaylistManager.Google.Config;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace JordanRhode.PlaylistManager.Google.Authentication;

public class GoogleAuthenticationHandler : DelegatingHandler
{
    private readonly GoogleOptions options;
	private readonly GoogleApplicationCredentials googleApplicationCredentials;
	private readonly IMemoryCache memoryCache;

	public GoogleAuthenticationHandler(IOptions<GoogleOptions> options, IOptions<GoogleApplicationCredentials> googleApplicationCredentials, IMemoryCache memoryCache)
	{
        this.options = options.Value;
        this.googleApplicationCredentials = googleApplicationCredentials.Value;
		this.memoryCache = memoryCache;
	}

	protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var accessToken = await GetAuthenticationTokenAsync();
        request.Headers.Authorization = new AuthenticationHeaderValue(
            scheme: "Bearer",
            parameter: accessToken
        );
        return await base.SendAsync(request, cancellationToken);
    }

	private async Task<string> GetAuthenticationTokenAsync()
    {
        const string cacheKey = "google_auth_token";
        if (!memoryCache.TryGetValue(cacheKey, out string? accessToken) || string.IsNullOrEmpty(accessToken))
        {
            using var request = new HttpRequestMessage()
            {
                Method = HttpMethod.Post,
                RequestUri = googleApplicationCredentials.TokenUri,
                Content = JsonContent.Create(new
                {
                    grant_type = options.GrantType,
                    assertion = GoogleUtilities.GenerateAuthenticationToken(googleApplicationCredentials, options)
                },
                MediaTypeHeaderValue.Parse("application/json"))
            };
            using var authenticationClient = new HttpClient();

            var response = await authenticationClient.SendAsync(request);
            accessToken = await response.Content.ReadAsStringAsync();

            if (!string.IsNullOrEmpty(accessToken))
            {
                memoryCache.Set(cacheKey, accessToken, new MemoryCacheEntryOptions()
                {
                    AbsoluteExpiration = DateTime.Now.AddMinutes(10)
                });
            }
        }
        return accessToken;
    }
}
