using MyAppBack.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using MyAppBack.Models.OrderAggregate;
using System.Linq;
using System;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MyAppBack.Models.Articles;

namespace MyAppBack.Data
{
  public class DataDbContext : DbContext
  {
    public DataDbContext(DbContextOptions<DataDbContext> options) : base(options) { }

    // public DbSet<User> Users { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<ProductType> ProductTypes { get; set; }
    public DbSet<ProductRegion> ProductRegions { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<DeliveryMethod> DeliveryMethods { get; set; }
    public DbSet<Article> Articles { get; set; }
    public DbSet<Comment> Comments { get; set; }



    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      base.OnModelCreating(modelBuilder);
      modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

      if (Database.ProviderName == "Microsoft.EntityFrameworkCore.Sqlite")
      {
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
          var dateTimeProperties = entityType.ClrType.GetProperties().Where(p => p.PropertyType == typeof(DateTimeOffset));

          foreach (var property in dateTimeProperties)
          {
            modelBuilder.Entity(entityType.Name).Property(property.Name).HasConversion(new DateTimeOffsetToBinaryConverter());
          }
        }
      }

      modelBuilder.Entity<Article>()
        .HasMany(c => c.Comments)
        .WithOne(e => e.Article)
        .HasForeignKey(k => k.ArticleId);

      modelBuilder.Entity<Comment>()
        .HasMany(c => c.SubComments)
        .WithOne(c => c.ParentComment)
        .HasForeignKey(c => c.ParentId);

    }



  }

}
