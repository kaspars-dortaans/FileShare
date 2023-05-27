using FileShare.Authorization;
using FileShare.Helpers;
using FileShare.Models;
using FileShare.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

// Add services to the container.
services.AddDbContext<DataContext>(opts => opts.UseSqlServer(builder.Configuration.GetConnectionString("DbConnectionString")));
services.AddControllers();

// configure strongly typed settings object
services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));

services.AddScoped<DataSeeding>();

services.AddScoped<IJwtUtils, JwtUtils>();
services.AddScoped<IUserService, UserService>();
services.AddScoped<ISettingsService, SettingsService>();

//Add automapper
services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();

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
