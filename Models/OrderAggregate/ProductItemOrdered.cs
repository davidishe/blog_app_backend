namespace MyAppBack.Models.OrderAggregate
{
  public class ProductItemOrdered
  {
    public ProductItemOrdered()
    {
    }

    public ProductItemOrdered(int productItemId, string productName, string pictureUrl, int guId)
    {
      ProductItemId = productItemId;
      Name = productName;
      PictureUrl = pictureUrl;
      GuId = guId;
    }

    public int ProductItemId { get; set; }
    public string Name { get; set; }
    public string PictureUrl { get; set; }
    public int GuId { get; set; }

  }
}