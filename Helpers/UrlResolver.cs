using AutoMapper;
using Microsoft.Extensions.Configuration;
using MyAppBack.Dtos;
using MyAppBack.Models;

namespace MyAppBack.Helpers
{
  public class UrlResolver : IValueResolver<Product, ProductToReturnDto, string>
  {
    public UrlResolver()
    {
    }

    private readonly IConfiguration _config;

    public UrlResolver(IConfiguration config)
    {
      _config = config;
    }

    public string Resolve(Product source, ProductToReturnDto destination, string destMember, ResolutionContext context)
    {
      if (!string.IsNullOrEmpty(source.PictureUrl))
      {
        return source.PictureUrl;
      }
      return null;
    }
  }
}