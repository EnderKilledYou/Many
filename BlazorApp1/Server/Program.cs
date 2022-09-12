using BlazorApp1.Client;
using BlazorApp1.Server.Hubs;
using BlazorApp1.Shared;
using ClipHunta2;
using Microsoft.AspNetCore.ResponseCompression;
using Serilog;
using ServiceStack.Data;
using ServiceStack.OrmLite;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews();
builder.Services.AddResponseCompression(opts =>
{
    opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
        new[] { "application/octet-stream" });
});
builder.Services.AddRazorPages();
builder.Services.AddSignalR();
builder.Logging.AddSerilog(new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .WriteTo.File("logs/testing.txt", rollingInterval: RollingInterval.Day).CreateLogger());
 
builder.Services.AddSingleton<IDbConnectionFactory>((a) => { 
    
    var dbcon = new OrmLiteConnectionFactory("db", SqliteDialect.Provider);
    using var db = dbcon.Open();
    TableUp.DoAllTableUps(db);
    return dbcon;

});
builder.Services.AddSingleton<TaskRaceTaskManager>();
builder.Services.AddServerSideBlazor(options =>
{
    options.DetailedErrors = false;
    options.DisconnectedCircuitMaxRetained = 100;
    options.DisconnectedCircuitRetentionPeriod = TimeSpan.FromMinutes(3);
    options.JSInteropDefaultCallTimeout = TimeSpan.FromMinutes(1);
    options.MaxBufferedUnacknowledgedRenderBatches = 10;
});
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
app.UseResponseCompression();

//app.UseAuthentication();



app.MapRazorPages();

    app.MapControllers();
    app.MapFallbackToFile("index.html");
    app.MapHub<StreamHub>("/streamhub");


app.Services.GetService<TaskRaceTaskManager>()?.AddLongTasker();
//app.MapHub<ChatHub>("/chathub");
app.Run();
