using System;
using System.Collections.Generic;

namespace MyAppBack.Models.OrderAggregate
{
  public class Order : BaseEntity
  {
    public Order()
    {
    }

    public Order(string byerEmail, Address shipToAddress, DeliveryMethod deliveryMethod, IReadOnlyList<OrderItem> orderItems, int subtotal, string paymentIntentId)
    {
      ByerEmail = byerEmail;
      ShipToAddress = shipToAddress;
      DeliveryMethod = deliveryMethod;
      OrderItems = orderItems;
      Subtotal = subtotal;
      PaymentIntentId = paymentIntentId;
    }

    public string ByerEmail { get; set; }
    public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.Now;
    public Address ShipToAddress { get; set; }
    public DeliveryMethod DeliveryMethod { get; set; }
    public IReadOnlyList<OrderItem> OrderItems { get; set; }
    public int Subtotal { get; set; }
    public OrderStatus Status { get; set; } = OrderStatus.Pending;
    public string PaymentIntentId { get; set; } = "0";

    public int GetTotal()
    {
      return Subtotal + DeliveryMethod.Price;
    }
  }
}