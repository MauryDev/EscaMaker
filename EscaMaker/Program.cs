using Blazored.LocalStorage;
using ClipLazor.Extention;
using EscaMaker;
using EscaMaker.Services;
using EscaMaker.View;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddSingleton(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddSingleton<EscaMaker.Services.PDFSchedule>();
builder.Services.AddSingleton<EscaMaker.Services.JsonExport>();
builder.Services.AddSingleton<EscaMaker.Services.AdminApiService>();
builder.Services.AddTransient<EscaMaker.Services.SchedulePeriodPdf>();
builder.Services.AddTransient<EscaMaker.Services.SchedulePersonPdf>();

builder.Services.AddTransient<BackgroundEventHandlerPDF>();

builder.Services.AddKeyedScoped<EscaMaker.Services.ScheduleTransfer.IScheduleTransfer, EscaMaker.Services.ScheduleTransfer.ScheduleLocalStorageActions>(ModeTransfer.LocalStorage);
builder.Services.AddKeyedScoped<EscaMaker.Services.ScheduleTransfer.IScheduleTransfer, EscaMaker.Services.ScheduleTransfer.ScheduleBrowserFileActions>(ModeTransfer.File);
builder.Services.AddKeyedScoped<EscaMaker.Services.ScheduleTransfer.IScheduleTransfer, EscaMaker.Services.ScheduleTransfer.ScheduleCloudActions>(ModeTransfer.Cloud);

builder.Services.AddMudServices();
builder.Services.AddBlazoredLocalStorageAsSingleton();
builder.Services.AddClipboard();

var app = builder.Build();

await app.RunAsync();
