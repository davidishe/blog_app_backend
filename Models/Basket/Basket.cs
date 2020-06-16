using System.Collections.Generic;

namespace MyAppBack.Models
{
  public class Basket
  {
    public Basket(string id)
    {
      Id = id;
    }

    public Basket()
    {
    }

    public string Id { get; set; }
    public List<BasketItem> Items { get; set; } = new List<BasketItem>();
    public int? DeliveryMethodId { get; set; }
    public string? ClientSecret { get; set; }
    public string? PaymentIntentId { get; set; }
    public int? ShippingPrice { get; set; }

  }
}