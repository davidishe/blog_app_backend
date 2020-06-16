using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using MyAppBack.Services.ResponseCacheService;

namespace MyAppBack.Helpers
{
  public class CachedAttribute : Attribute, IAsyncActionFilter
  {
    private readonly int _timeToLive;
    public CachedAttribute(int timeToLive)
    {
      _timeToLive = timeToLive;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
      var cacheService = context.HttpContext.RequestServices.GetRequiredService<IResponseCacheService>();
      var cacheKey = GenereteCacheKeyFromRequest(context.HttpContext.Request);
      var cacheReponse = await cacheService.GetCacheResponseAsync(cacheKey);

      if (!string.IsNullOrEmpty(cacheReponse))
      {
        var contentResult = new ContentResult
        {
          Content = cacheReponse,
          ContentType = "application/json",
          StatusCode = 200
        };

        context.Result = contentResult;
        return;
      }

      var executedContext = await next(); // move to controller

      if (executedContext.Result is OkObjectResult okObjectResult)
      {
        await cacheService.CacheResponseService(cacheKey, okObjectResult.Value, TimeSpan.FromSeconds(_timeToLive));
      }
    }

    private string GenereteCacheKeyFromRequest(HttpRequest request)
    {
      var keyBuilder = new StringBuilder();
      keyBuilder.Append($"{request.Path}");

      foreach (var (key, value) in request.Query.OrderBy(x => x.Key))
      {
        keyBuilder.Append($"|{key}-{value}");
      };

      return keyBuilder.ToString();

    }
  }
}