using System.Threading.Tasks;
using MyAppBack.Models;
using MyAppBack.Models.OrderAggregate;

namespace MyAppBack.Services.PaymentService
{
  public interface IPaymentService
  {
    Task<Basket> CreateOrUpdatePaymentIntent(string basketId);
    Task<Order> UpdateOrderPaymentSucceded(string paymentInentId);
    Task<Order> UpdateOrderPaymentFailed(string paymentInentId);


  }
}