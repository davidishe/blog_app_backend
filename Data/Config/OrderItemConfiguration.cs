using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyAppBack.Models.OrderAggregate;

namespace MyAppBack.Data.Config
{
  public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
  {
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
      builder.OwnsOne(i => i.ItemOrdered, io => { io.WithOwner(); });
      builder.Property(i => i.Price);
    }
  }
}
