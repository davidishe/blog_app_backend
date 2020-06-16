using System;

namespace MyAppBack.Models
{
  public class Product : BaseEntity
  {
    public string Name { get; set; }
    public int Price { get; set; }
    public string PictureUrl { get; set; }
    public string? Description { get; set; }
    public ProductType? Type { get; set; }
    public int? ProductTypeId { get; set; }
    public ProductRegion? Region { get; set; }
    public int? ProductRegionId { get; set; }
    public int? Quantity { get; set; }
    public DateTime? EnrolledDate { get; set; } = DateTime.Now;
    public int GuId { get; set; }

  }
}
