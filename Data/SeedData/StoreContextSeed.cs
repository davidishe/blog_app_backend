using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MyAppBack.Models.Articles;

namespace MyAppBack.Data.SeedData
{
  public class StoreContextSeed
  {
    public static async Task SeedDataAsync(DataDbContext context, ILoggerFactory loggerFactory)
    {
      try
      {

        var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        if (!context.Articles.Any())
        {
          var articlesData = File.ReadAllText(path + @"/Data/SeedData/Source/articles.json");
          var articles = JsonSerializer.Deserialize<List<Article>>(articlesData);
          foreach (var item in articles)
          {
            context.Articles.Add(item);
          }
          await context.SaveChangesAsync();
        }

        if (!context.Comments.Any())
        {
          var commentData = File.ReadAllText(path + @"/Data/SeedData/Source/comments.json");
          var comments = JsonSerializer.Deserialize<List<Comment>>(commentData);
          foreach (var item in comments)
          {
            context.Comments.Add(item);
          }
          await context.SaveChangesAsync();
        }


      }
      catch (Exception ex)
      {
        var logger = loggerFactory.CreateLogger<StoreContextSeed>();
        logger.LogError(ex.Message);
      }
    }


  }
}