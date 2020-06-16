namespace MyAppBack.Models
{
  public class BasketItem
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public int Price { get; set; }
    public int Quantity { get; set; }
    public string PictureUrl { get; set; }
    public string Type { get; set; }
    public string Region { get; set; }
    public int GuId { get; set; }
  }
}