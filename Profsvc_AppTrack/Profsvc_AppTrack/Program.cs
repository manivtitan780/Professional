#region Header

// /*****************************************
// Copyright:           Titan-Techs.
// Location:            Newtown, PA, USA
// Solution:            Profsvc_AppTrack
// Project:             Profsvc_AppTrack
// File Name:           Program.cs
// Created By:          Narendra Kumaran Kadhirvelu, Jolly Joseph Paily, DonBosco Paily, Mariappan Raja
// Created On:          11-22-2023 20:50
// Last Updated On:     12-12-2023 16:8
// *****************************************/

#endregion

#region Using

using System.IO.Compression;

using Microsoft.AspNetCore.ResponseCompression;

#endregion

WebApplicationBuilder _builder = WebApplication.CreateBuilder(args);

// Add services to the container.
_builder.Services.AddRazorComponents()
        .AddInteractiveServerComponents();
_builder.Services.AddSingleton<Start>();
_builder.Services.AddHttpContextAccessor();
_builder.Services.AddHttpClient();
_builder.Services.AddBlazoredLocalStorage();   // Local storage
_builder.Services.AddBlazoredSessionStorage(); // Session storage
_builder.Services.AddMemoryCache();
_builder.Services.AddSignalR(e =>
                             {
                                 e.MaximumReceiveMessageSize = 10485760;
                                 e.EnableDetailedErrors = true;
                             });
_builder.Services.AddScoped<SfDialogService>();
_builder.Services.AddSyncfusionBlazor();
_builder.Services.AddServerSideBlazor().AddCircuitOptions(option => { option.DetailedErrors = true; });
_builder.Services.AddResponseCompression(options =>
                                         {
                                             options.Providers.Add<BrotliCompressionProvider>();
                                             options.Providers.Add<GzipCompressionProvider>();
                                             options.MimeTypes =
                                                 ResponseCompressionDefaults.MimeTypes.Concat(new[] {"image/svg+xml"});
                                         });
_builder.Services.Configure<BrotliCompressionProviderOptions>(options => { options.Level = CompressionLevel.Fastest; });
_builder.Services.Configure<GzipCompressionProviderOptions>(options => { options.Level = CompressionLevel.Fastest; });

Log.Logger = new LoggerConfiguration().MinimumLevel.Error().WriteTo.File("log/errorLog.txt", rollingInterval: RollingInterval.Day).CreateLogger();
_builder.Services.AddLogging(logging =>
                             {
                                 logging.ClearProviders();
                                 logging.AddSerilog(Log.Logger);
                             });

WebApplication _app = _builder.Build();
SyncfusionLicenseProvider.RegisterLicense("MjcwMjk1M0AzMjMzMmUzMDJlMzBkb3lEc1VuaG9PNGo2NlVoMG9UNGZUcUdIOCtRY3NYUGZjbEVrRWt0SUhNPQ==");

if (_app.Environment.EnvironmentName is not ("Development" or "India" or "US"))
{
    _app.UseExceptionHandler("/Error", true);
}

_app.UseHttpsRedirection();

_app.UseStaticFiles();
_app.UseAntiforgery();

_app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

_app.UseResponseCompression();

IMemoryCache _memoryCache = new MemoryCache(new MemoryCacheOptions());

Start.MemCache = _memoryCache;

Start.ApiHost = _app.Configuration.GetValue(typeof(string), "APIHost")?.ToString();
Start.ConnectionString = _app.Configuration.GetConnectionString("DBConnect");
Start.UploadsPath = _builder.Environment.WebRootPath;
Start.JsonFilePath = _app.Configuration.GetValue(typeof(string), "JsonPath")?.ToString();
Start.SetCache();

_app.Run();