using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using MyAppBack.Data.Repos.BasketRepository;
using MyAppBack.Data.Spec;
using MyAppBack.Data.UnitOfWork;
using MyAppBack.Models;
using MyAppBack.Models.OrderAggregate;
using Stripe;
using Order = MyAppBack.Models.OrderAggregate.Order;
using Product = MyAppBack.Models.Product;

namespace MyAppBack.Services.PaymentService
{
  public class PaymentService : IPaymentService
  {
    private readonly IBasketRepository _basketRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IConfiguration _config;
    public PaymentService(IBasketRepository basketRepository, IUnitOfWork unitOfWork, IConfiguration config)
    {
      _config = config;
      _unitOfWork = unitOfWork;
      _basketRepository = basketRepository;
    }

    public async Task<Basket> CreateOrUpdatePaymentIntent(string basketId)
    {
      StripeConfiguration.ApiKey = _config["StripeSettings:SecretKey"];
      var basket = await _basketRepository.GetBasketAsync(basketId);
      var shippingPrice = 0;

      if (basket == null) return null;

      if (basket.DeliveryMethodId.HasValue)
      {

        var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync((int)basket.DeliveryMethodId);
        shippingPrice = deliveryMethod.Price;

        foreach (var item in basket.Items)
        {
          var productItem = await _unitOfWork.Repository<Product>().GetByIdAsync(item.Id);
          if (item.Price != productItem.Price)
          {
            item.Price = productItem.Price;
          }
        }

        var service = new PaymentIntentService();
        PaymentIntent intent;

        if (string.IsNullOrEmpty(basket.PaymentIntentId))
        {
          var options = new PaymentIntentCreateOptions
          {
            Amount = (long)basket.Items.Sum(i => (i.Quantity * i.Price) * 100) + (long)(shippingPrice * 100),
            Currency = "usd",
            PaymentMethodTypes = new List<string> { "card" }
          };

          intent = await service.CreateAsync(options);
          basket.PaymentIntentId = intent.Id;
          basket.ClientSecret = intent.ClientSecret;
        }
        else
        {
          var options = new PaymentIntentUpdateOptions
          {
            Amount = (long)basket.Items.Sum(i => (i.Quantity * i.Price) * 100) + (long)(shippingPrice * 100),
          };
          await service.UpdateAsync(basket.PaymentIntentId, options);
        }

        await _basketRepository.UpdateBasketAsync(basket);
        return basket;
      }

      return null;
    }

    public async Task<Order> UpdateOrderPaymentFailed(string paymentInentId)
    {
      var spec = new OrderByPaymentIntentIdSpecification(paymentInentId);
      var order = await _unitOfWork.Repository<Order>().GetEntityWithSpec(spec);
      if (order == null) return null;
      order.Status = OrderStatus.PaymentFailed;
      _unitOfWork.Repository<Order>().Update(order);
      await _unitOfWork.Complete();
      return order;
    }

    public async Task<Order> UpdateOrderPaymentSucceded(string paymentInentId)
    {
      var spec = new OrderByPaymentIntentIdSpecification(paymentInentId);
      var order = await _unitOfWork.Repository<Order>().GetEntityWithSpec(spec);
      if (order == null) return null;
      order.Status = OrderStatus.PaymentReceived;
      _unitOfWork.Repository<Order>().Update(order);
      await _unitOfWork.Complete();
      return order;
    }
  }
}