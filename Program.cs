using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MyAppBack.Data;
using MyAppBack.Data.Contexts;
using MyAppBack.Data.SeedData;
using MyAppBack.Identity;

namespace MyAppBack
{
  public class Program
  {
    public static async Task Main(string[] args)
    {
      var host = CreateHostBuilder(args).Build();

      using (var scope = host.Services.CreateScope())
      {

        var services = scope.ServiceProvider;
        var loggerFactory = services.GetRequiredService<ILoggerFactory>();
        try
        {
          var context = services.GetRequiredService<DataDbContext>();
          await StoreContextSeed.SeedDataAsync(context, loggerFactory);
          await context.Database.MigrateAsync();

          var userManager = services.GetRequiredService<UserManager<AppUser>>();
          var identityContext = services.GetRequiredService<AppIdentityDbContext>();
          await identityContext.Database.MigrateAsync();
          await AppIdentityDbContextSeed.SeedUsersAsync(userManager, loggerFactory);

        }
        catch (Exception ex)
        {
          var logger = services.GetRequiredService<ILogger<Program>>();
          logger.LogError(ex, "An error occured during migrtation");
        }
      }

      host.Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
              webBuilder.UseStartup<Startup>().
                UseUrls("http://localhost:5030");
            });
  }
}
