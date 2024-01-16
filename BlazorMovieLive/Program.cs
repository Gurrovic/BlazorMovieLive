using Blazored.LocalStorage;
using BlazorMovieLive.Client;
using BlazorMovieLive.Client.Services;
using BlazorMovieLive.Client.Services.Contracts;
using BlazorMovieLive.Client.Utility;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using System.Net.Http.Headers;


var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddBlazoredLocalStorage();
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
builder.Services.AddScoped<AuthenticationDelegatingHandler>();

builder.Services.AddHttpClient("InternalApi", client =>
{    
    client.BaseAddress = new Uri("https://localhost:7213/");
    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
}).AddHttpMessageHandler<AuthenticationDelegatingHandler>();


builder.Services.AddHttpClient("ExternalApi", client =>
{
    client.BaseAddress = new Uri("https://api.themoviedb.org/3/");
    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));    
});

builder.Services.AddScoped<TMDBCLient>();
builder.Services.AddScoped<IAuthService, AuthService>();

await builder.Build().RunAsync();