using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyAppBack.Data.Repos.BasketRepository;
using MyAppBack.Data.Repos.GenericRepository;
using MyAppBack.Data.Spec;
using MyAppBack.Data.UnitOfWork;
using MyAppBack.Models;
using MyAppBack.Models.OrderAggregate;
using MyAppBack.Services.PaymentService;

namespace MyAppBack.Services.OrderService
{
  public class OrderService : IOrderService
  {
    private readonly IBasketRepository _basketRepo;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPaymentService _paymentService;
    public OrderService(IUnitOfWork unitOfWork, IBasketRepository basketRepo, IPaymentService paymentService)
    {
      _paymentService = paymentService;
      _unitOfWork = unitOfWork;
      _basketRepo = basketRepo;
    }

    public async Task<Order> CreateOrderAsync(string byerEmail, int deliveryMethodId, string basketId, Address shipingAddress)
    {
      // get basket form repo
      var basket = await _basketRepo.GetBasketAsync(basketId);

      // get items from the product repo
      var items = new List<OrderItem>();
      foreach (var item in basket.Items)
      {
        var productItem = await _unitOfWork.Repository<Product>().GetByIdAsync(item.Id);
        var itemOrdered = new ProductItemOrdered(productItem.Id, productItem.Name, productItem.PictureUrl, productItem.GuId);
        var orderItem = new OrderItem(itemOrdered, productItem.Price, item.Quantity, item.GuId);
        items.Add(orderItem);
      }

      // get delivery method from repo
      var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(deliveryMethodId);

      // calc subtotal
      var subtotal = items.Sum(item => (item.Price * item.Quantity));

      // check paymentIntentId already exists
      var spec = new OrderByPaymentIntentIdSpecification(basket.PaymentIntentId);
      var existingOrder = await _unitOfWork.Repository<Order>().GetEntityWithSpec(spec);
      if (existingOrder != null)
      {
        _unitOfWork.Repository<Order>().Delete(existingOrder);
        await _paymentService.CreateOrUpdatePaymentIntent(basket.PaymentIntentId);
      }

      // create order
      var order = new Order(byerEmail, shipingAddress, deliveryMethod, items, subtotal, basket.PaymentIntentId);
      _unitOfWork.Repository<Order>().Add(order);

      // TO DO: save to db
      var result = await _unitOfWork.Complete();
      if (result <= 0) return null;

      // return order
      return order;
    }

    public async Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync()
    {
      return await _unitOfWork.Repository<DeliveryMethod>().ListAllAsync();
    }

    public async Task<Order> GetOrderById(int id, string byerEmail)
    {
      var spec = new OrderWithItemsAndOrderingSpecification(id, byerEmail);
      return await _unitOfWork.Repository<Order>().GetEntityWithSpec(spec);

    }

    public async Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string byerEmail)
    {
      var spec = new OrderWithItemsAndOrderingSpecification(byerEmail);
      return await _unitOfWork.Repository<Order>().ListAsync(spec);
    }
  }
}