using FinTracker;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using Syncfusion.Blazor;

// Register Syncfusion license
Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MzkzNTY1N0AzMzMwMmUzMDJlMzAzYjMzMzAzYlZrNVBKdytZeEgvN2ErRUIyM3U3R3FodnFjUDlDSW90QVpuY0s5ZzI1SUU9");

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddSingleton(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });


//Mudblazor
builder.Services.AddMudServices();

//Syncfusion
builder.Services.AddSyncfusionBlazor();

//Toast
builder.Services.AddScoped<ToastService>();

await builder.Build().RunAsync();
