using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using MyAppBack.Identity;

namespace MyAppBack.Data.SeedData
{
  public class AppIdentityDbContextSeed
  {
    public static async Task SeedUsersAsync(UserManager<AppUser> userManager, ILoggerFactory loggerFactory)
    {
      try
      {

        if (!userManager.Users.Any())
        {
          var user = new AppUser
          {
            DisplayName = "David",
            Email = "david@david.com",
            UserName = "david@david.com",
            UserPhoto = "assets/users/hopkins.png",
            UserDescription = "Меня направляет божество и все такое",
            Address = new Address
            {
              FirstName = "David",
              LastName = "Earth",
              Street = "Bayker",
              City = "London",
              House = "22",
              ZipCode = "444213"
            }
          };

          await userManager.CreateAsync(user, "Pa$$w0rd");

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