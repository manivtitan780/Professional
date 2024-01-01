#region Header

// /*****************************************
// Copyright:           Titan-Techs.
// Location:            Newtown, PA, USA
// Solution:            Profsvc_AppTrack
// Project:             Profsvc_AppTrack
// File Name:           Program.cs
// Created By:          Narendra Kumaran Kadhirvelu, Jolly Joseph Paily, DonBosco Paily, Mariappan Raja
// Created On:          11-22-2023 20:50
// Last Updated On:     1-1-2024 15:33
// *****************************************/

#endregion

#region Using

using System.IO.Compression;

using Azure.Core;

using Microsoft.AspNetCore.ResponseCompression;

using Microsoft.AspNetCore.Http;
using Profsvc_AppTrack.Components;


#endregion

WebApplicationBuilder _builder = WebApplication.CreateBuilder(args);

// Add services to the container.
_builder.Services.AddRazorComponents()
		.AddInteractiveServerComponents();
_builder.Services.AddSingleton<Start>();
_builder.Services.AddHttpContextAccessor();
_builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
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
												 ResponseCompressionDefaults.MimeTypes.Concat(new[] { "image/svg+xml" });
										 });

_builder.Services.Configure<BrotliCompressionProviderOptions>(options => { options.Level = CompressionLevel.Fastest; });
_builder.Services.Configure<GzipCompressionProviderOptions>(options => { options.Level = CompressionLevel.Fastest; });

Log.Logger = new LoggerConfiguration().MinimumLevel.Error().WriteTo.File("log/errorLog.txt", rollingInterval: RollingInterval.Day).CreateLogger();
_builder.Services.AddLogging(logging =>
							 {
								 logging.ClearProviders();
								 logging.AddSerilog(Log.Logger);
							 });
IConfigurationSection _redisConfig = _builder.Configuration.GetSection("Redis");
string _hostName = _redisConfig["HostName"];
int _sslPort = _redisConfig["SslPort"].ToInt32();
string _access = _redisConfig["Access"];

_builder.Services.AddSingleton(new RedisService(_hostName, _sslPort, _access));

WebApplication _app = _builder.Build();

SyncfusionLicenseProvider.RegisterLicense("Mjk4MzA3NEAzMjM0MmUzMDJlMzBrcGdpN2lWdzkvd2dhV05UVmh3VWdKN1JBZUhXUHFMUUVSRTJoZW84QmVZPQ==");

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

//IMemoryCache _memoryCache = new MemoryCache(new MemoryCacheOptions());
//Start.MemCache = _memoryCache;

Start.ApiHost = _app.Configuration.GetValue(typeof(string), "APIHost")?.ToString();
Start.ConnectionString = _app.Configuration.GetConnectionString("DBConnect");
Start.UploadsPath = _builder.Environment.WebRootPath;
Start.JsonFilePath = _app.Configuration.GetValue(typeof(string), "JsonPath")?.ToString();
Start.SetCache();

_app.Run();
