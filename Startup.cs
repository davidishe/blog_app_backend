using System.IO;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using MyAppBack.Data;
using MyAppBack.Data.Contexts;
using MyAppBack.Extensions;
using MyAppBack.Middleware;
using StackExchange.Redis;

namespace MyAppBack
{
  public class Startup
  {
    public Startup(IConfiguration configuration)
    {
      _config = configuration;
    }

    public IConfiguration _config { get; }


    public void ConfigureDevelopmentServices(IServiceCollection services)
    {
      services.AddDbContext<DataDbContext>(options => options.UseSqlite(_config.GetConnectionString("DefaultConnection")));
      services.AddDbContext<AppIdentityDbContext>(options => options.UseSqlite(_config.GetConnectionString("IdentityConnection")));
      // services.AddIdentity<AppUser, IdentityRole>().AddEntityFrameworkStores<AppIdentityDbContext>().AddDefaultTokenProviders();

      ConfigureServices(services);
    }

    public void ConfigureProductionServices(IServiceCollection services)
    {
      services.AddDbContext<DataDbContext>(options => options.UseMySql(_config.GetConnectionString("DefaultConnection")));
      services.AddDbContext<AppIdentityDbContext>(options => options.UseMySql(_config.GetConnectionString("IdentityConnection")));


      ConfigureServices(services);
    }

    public void ConfigureServices(IServiceCollection services)
    {

      services.AddSingleton<IConnectionMultiplexer>(c =>
      {
        var configuration = ConfigurationOptions.Parse(_config.GetConnectionString("Redis"), true);
        return ConnectionMultiplexer.Connect(configuration);
      });

      services.AddControllers()
        .AddNewtonsoftJson(opt =>
        {
          opt.SerializerSettings.ReferenceLoopHandling =
          Newtonsoft.Json.ReferenceLoopHandling.Ignore;
        });

      services.AddCors();

      services.AddAutoMapper(typeof(Startup));

      services.AddIdentityServices(_config);
      services.AddApplicationServices();

    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {

      app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
      app.UseMiddleware<ExceptionMiddleware>();
      app.UseStatusCodePagesWithReExecute("/errors/{0}");

      app.UseRouting();
      app.UseAuthentication();
      app.UseAuthorization();
      app.UseDefaultFiles();
      app.UseStaticFiles();
      app.UseStaticFiles(new StaticFileOptions
      {
        FileProvider = new PhysicalFileProvider(
          Path.Combine(Directory.GetCurrentDirectory(), "assets")
        ),
        RequestPath = "/assets"
      });
      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllers();
        endpoints.MapFallbackToController("Index", "Fallback");
      });
    }
  }
}
