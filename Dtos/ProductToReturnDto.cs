using System;

namespace MyAppBack.Dtos
{
  public class ProductToReturnDto
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public int Price { get; set; }
    public string PictureUrl { get; set; }
    public string? Description { get; set; }
    public string Type { get; set; }
    public int ProductTypeId { get; set; }
    public string Region { get; set; }
    public int ProductRegionId { get; set; }
    public int? Quantity { get; set; }
    public bool? ProductIsSelected { get; set; }
    public DateTime? EnrolledDate { get; set; }
    public int? GuId { get; set; }
    public int? OrderId { get; set; }
  };
}