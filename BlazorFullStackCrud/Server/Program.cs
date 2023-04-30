global using BlazorFullStackCrud.Shared;
global using Microsoft.EntityFrameworkCore;
global using BlazorFullStackCrud.Server.Data;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using BlazorFullStackCrud.Client;
using BlazorFullStackCrud.Server.BlockchainServices;
using Nethereum.Web3;

var builder = WebApplication.CreateBuilder(args);

// Create an instance of Blockchain class and set its properties
var blockchain = new Blockchain
{ 
    AlchemyApiKey = "P_LoX-4Ip9QIsaY9ULhABUuziQw0Q6bm",
    MetaMaskPrivateKey = "0x657ffcb64f32ff984a3fb43d21e177f1c925dc142f5ab1bf300489d64c1e9dbe",
    NodeWebsocketUrl = "wss://eth-sepolia.g.alchemy.com/v2/P_LoX-4Ip9QIsaY9ULhABUuziQw0Q6bm",
    NodeHttpsUrl= "https://eth-sepolia.g.alchemy.com/v2/P_LoX-4Ip9QIsaY9ULhABUuziQw0Q6bm"

};

// Regiter the instance of Blockchain class as a singleton service
builder.Services.AddSingleton(blockchain);

// daca las comentat aici, porneste, dar am un mesaj in debugging console in chrome
// daca decomentez am o exceptie si nu porneste
//builder.Services.AddScoped<IBlockchainServices, BlockchainServices>();



try
{
    builder.Services.AddSingleton(x => new Web3(blockchain.NodeWebsocketUrl));
}
catch (Exception ex)
{
    Console.WriteLine($"An error occurred while creating the Web3 instance: {ex.Message}");
}


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
