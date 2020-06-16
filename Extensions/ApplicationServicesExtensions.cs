using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using MyAppBack.Data.Repos;
using MyAppBack.Data.Repos.ArticleRepository;
using MyAppBack.Data.Repos.BasketRepository;
using MyAppBack.Data.Repos.GenericRepository;
using MyAppBack.Data.UnitOfWork;
using MyAppBack.Errors;
using MyAppBack.Services.OrderService;
using MyAppBack.Services.PaymentService;
using MyAppBack.Services.ResponseCacheService;
using MyAppBack.Services.TokenService;

namespace MyAppBack.Extensions
{
  public static class ApplicationServicesExtensions
  {
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
      services.AddSingleton<IResponseCacheService, ResponseCacheService>();
      services.AddScoped<ITokenService, TokenService>();
      services.AddScoped<IPaymentService, PaymentService>();
      services.AddScoped<IOrderService, OrderService>();

      services.AddScoped<IProductRepository, ProductRepository>();
      services.AddScoped<IArticleRepository, ArticleRepository>();
      services.AddScoped<IBasketRepository, BasketRepository>();
      services.AddScoped(typeof(IGenericRepository<>), (typeof(GenericRepository<>)));
      services.AddScoped<IUnitOfWork, UnitOfWork>();


      services.Configure<ApiBehaviorOptions>(options =>
      {
        options.InvalidModelStateResponseFactory = actionContext =>
        {
          var errors = actionContext.ModelState
            .Where(e => e.Value.Errors.Count > 0)
            .SelectMany(x => x.Value.Errors)
            .Select(x => x.ErrorMessage).ToArray();

          var errorResponse = new ApiValidationErrorResponse()
          {
            Errors = errors
          };

          return new BadRequestObjectResult(errorResponse);

        };

      });

      return services;

    }
  }
}