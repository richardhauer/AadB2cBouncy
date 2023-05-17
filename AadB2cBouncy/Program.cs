using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using AadB2cBouncy;
using Microsoft.Authentication.WebAssembly.Msal;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

var msalOpts = new MsalAuthenticationOptions();
builder.Configuration.Bind( "AzureAdB2C", msalOpts );
builder.Services.AddSingleton( msalOpts );

builder.Services.AddMsalAuthentication(options =>
{
	builder.Configuration.Bind("AzureAdB2C", options.ProviderOptions.Authentication);

	/* These lines are additional to the template */
	options.ProviderOptions.DefaultAccessTokenScopes = new List<string> { "https://dev.teeg.cloud/api/all-apis" };
	options.ProviderOptions.Cache.CacheLocation = "localStorage";
	options.ProviderOptions.LoginMode = "redirect";
} );

await builder.Build().RunAsync();
