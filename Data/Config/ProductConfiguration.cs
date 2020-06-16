using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyAppBack.Models;

namespace MyAppBack.Data.Config
{
  public class ProductConfiguration : IEntityTypeConfiguration<Product>
  {
    public void Configure(EntityTypeBuilder<Product> builder)
    {
      builder.Property(p => p.Id).IsRequired();
      builder.Property(p => p.Name).IsRequired().HasMaxLength(100);
      builder.Property(p => p.Description).IsRequired();
      builder.Property(p => p.Price).IsRequired();
      builder.Property(p => p.PictureUrl).IsRequired();
      builder.Property(p => p.Quantity);
      builder.Property(p => p.GuId).IsRequired();
      builder.Property(p => p.EnrolledDate).IsRequired();

      builder.HasOne(b => b.Type).WithMany().HasForeignKey(p => p.ProductTypeId);
      builder.HasOne(b => b.Region).WithMany().HasForeignKey(p => p.ProductRegionId);

    }
  }
}