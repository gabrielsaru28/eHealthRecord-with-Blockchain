global using BlazorFullStackCrud.Shared;
global using Microsoft.EntityFrameworkCore;
global using BlazorFullStackCrud.Server.Data;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using BlazorFullStackCrud.Client;
using BlazorFullStackCrud.Server.BlockchainServices;
using Nethereum.Web3;

var builder = WebApplication.CreateBuilder(args);

// Load values from appsettings.json
var blockchainSettings = builder.Configuration.GetSection("Blockchain").Get<Blockchain>();

// Register the instance of Blockchain class as a singleton service
builder.Services.AddSingleton(blockchainSettings);

// This registers the IBlockchainServices interface and the BlockchainServices implementation with the DI container as a scoped service.
// The AddScoped method is used to register the IBlockchainServices interface and the BlockchainServices implementation with the DI container as a scoped service. The lambda expression provided as the second argument creates a new instance of the BlockchainServices class and passes in the required dependencies, including the DataContext instance obtained from the DI container using the GetRequiredService method.
builder.Services.AddScoped<IBlockchainServices, BlockchainServices>();


// Register the Web3 instance using the values from the Blockchain settings
builder.Services.AddSingleton(x => new Web3(blockchainSettings.NodeWebsocketUrl));

// Check if the IBlockchainServices service is registered in the DI container
if (builder.Services.All(s => s.ServiceType != typeof(IBlockchainServices)))
{
    Console.WriteLine("The IBlockchainServices service is not registered in the DI container.");
}
else
{
    Console.WriteLine("The IBlockchainServices service is registered in the DI container.");
}

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddControllers().AddJsonOptions(x =>
    x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

builder.Services.AddRazorPages();

// We register this 
builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseBlazorFrameworkFiles();
app.UseStaticFiles();
app.UseRouting();

app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();