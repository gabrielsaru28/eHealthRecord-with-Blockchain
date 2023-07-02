global using Microsoft.AspNetCore.Components.Authorization;
global using BlazorFullStackCrud.Client.Services.HealthRecordService;
global using BlazorFullStackCrud.Shared;
global using Blazored.LocalStorage;

using BlazorFullStackCrud.Client;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Blazored.Toast;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddBlazoredToast();
builder.Services.AddOptions();
builder.Services.AddAuthorizationCore();
// add for the Service
// Whenever someone wants to inject the IHealthRecordService we want to use the HealthRecord Service implementation class
builder.Services.AddScoped<IHealthRecordService, HealthRecordService>();

await builder.Build().RunAsync();