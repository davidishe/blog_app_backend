namespace MyAppBack.Models.OrderAggregate
{
  public class OrderItem : BaseEntity
  {
    public OrderItem()
    {
    }

    public OrderItem(ProductItemOrdered itemOrdered, int price, int quantity, int guId)
    {
      ItemOrdered = itemOrdered;
      Price = price;
      GuId = guId;
      Quantity = quantity;
    }

    public ProductItemOrdered ItemOrdered { get; set; }
    public int Price { get; set; }
    public int Quantity { get; set; }
    public int GuId { get; set; }

  }
}