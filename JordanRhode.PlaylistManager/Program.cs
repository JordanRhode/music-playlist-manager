using JordanRhode.PlaylistManager.Google.Config;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOptions<GoogleApplicationCredentials>()
	.Bind(builder.Configuration.GetSection("GoogleApplicationCredentials"))
	.ValidateDataAnnotations()
	.ValidateOnStart();
builder.Services.AddOptions<GoogleOptions>()
	.Bind(builder.Configuration.GetSection("GoogleOptions"))
	.ValidateDataAnnotations()
	.ValidateOnStart();

builder.Services.AddHealthChecks();

var app = builder.Build();

app.UseHttpsRedirection();

app.MapHealthChecks("/health");

app.Run();
