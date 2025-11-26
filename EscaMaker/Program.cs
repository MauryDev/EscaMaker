using Blazored.LocalStorage;
using ClipLazor.Extention;
using EscaMaker;
using EscaMaker.Services.Contracts;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddSingleton(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddSingleton<EscaMaker.Services.PDFEscala>();
builder.Services.AddSingleton<EscaMaker.Services.JsonExport>();
builder.Services.AddSingleton<IEscalaDataTransferService,EscaMaker.Services.EscalaDataTransferService>();

builder.Services.AddSingleton<EscaMaker.Services.AdminApiService>();

builder.Services.AddMudServices();
builder.Services.AddBlazoredLocalStorageAsSingleton();
builder.Services.AddClipboard();

var app = builder.Build();

await app.RunAsync();
