using FileShare.Authorization;
using FileShare.Helpers;
using FileShare.Models;
using FileShare.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Azure;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

// Add services to the container.
services.AddDbContext<DataContext>(opts => opts.UseSqlServer(builder.Configuration.GetConnectionString("SqlDataBase")));
services.AddControllers();

// configure strongly typed settings object
services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));

//azure storage
services.AddAzureClients(configureClients =>
{
    configureClients.AddBlobServiceClient(builder.Configuration.GetConnectionString("AzureStorage"));
});
services.AddScoped<IAzureStorageHelper, AzureStorageHelper>();

//data seeding
services.AddScoped<DataSeeding>();

//db services
services.AddScoped<IJwtUtils, JwtUtils>();
services.AddScoped<IUserService, UserService>();
services.AddScoped<ISettingsService, SettingsService>();
services.AddScoped<IFileService, FileService>();

//Add automapper
services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();

//dev data seeding
if (app.Environment.IsDevelopment())
{
    var scope = app.Services.CreateScope();
    var dataSeeder = (DataSeeding?)scope.ServiceProvider.GetService(typeof(DataSeeding));
    dataSeeder?.SeedData();
}

app.UseRouting();

// global error handler
app.UseMiddleware<ErrorHandlerMiddleware>();

// custom jwt auth middleware
app.UseMiddleware<JwtMiddleware>();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "api/{controller=Home}/{action=Index}/{id?}");
});

app.UseSpa(spa =>
{
    spa.Options.SourcePath = "client-app";

    if (app.Environment.IsDevelopment())
    {
        var uri = $"http://{builder.Configuration["DevelopmentSettings:ViteHost"]}:{builder.Configuration["DevelopmentSettings:VitePort"]}";
        spa.UseProxyToSpaDevelopmentServer(uri);
    }
});

app.Run();
