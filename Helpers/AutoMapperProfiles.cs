using AutoMapper;
using MyAppBack.Dtos;
using MyAppBack.Dtos.Articles;
using MyAppBack.Models;
using MyAppBack.Models.Articles;
using MyAppBack.Models.OrderAggregate;

namespace MyAppBack.Helpers
{
  public class AutoMapperProfiles : Profile
  {

    public AutoMapperProfiles()
    {
      CreateMap<Product, ProductToReturnDto>()
        .ForMember(d => d.Type, m => m.MapFrom(s => s.Type.Name))
        .ForMember(d => d.Region, m => m.MapFrom(s => s.Region.Name))
        .ForMember(d => d.PictureUrl, m => m.MapFrom<UrlResolver>());

      CreateMap<MyAppBack.Identity.Address, AddressDto>().ReverseMap();
      CreateMap<BasketDto, Basket>();
      CreateMap<BasketItemDto, BasketItem>();
      CreateMap<AddressDto, MyAppBack.Models.OrderAggregate.Address>();
      CreateMap<Order, OrderToReturnDto>()
        .ForMember(d => d.DeliveryMethod, m => m.MapFrom(s => s.DeliveryMethod.ShortName))
        .ForMember(d => d.DeliveryPrice, m => m.MapFrom(s => s.DeliveryMethod.Price));

      CreateMap<OrderItem, OrderItemDto>()
        .ForMember(d => d.ProductId, m => m.MapFrom(s => s.ItemOrdered.ProductItemId))
        .ForMember(d => d.Name, m => m.MapFrom(s => s.ItemOrdered.Name))
        .ForMember(d => d.GuId, m => m.MapFrom(s => s.ItemOrdered.GuId))
        .ForMember(d => d.PictureUrl, m => m.MapFrom(s => s.ItemOrdered.PictureUrl))
        .ForMember(d => d.PictureUrl, m => m.MapFrom<OrderItemUrlResolver>());

      // CreateMap<ArticleComment, CommentDto>();
      // .ForMember(d => d.CommentReplyDto, m => m.MapFrom(s => s.ArticleCommentReply)).ReverseMap();

      CreateMap<Article, ArticleToReturnDto>()
        .ForMember(d => d.Comments, m => m.MapFrom(s => s.Comments.ToArray()));

      CreateMap<Comment, CommentToReturnDto>();

    }
  }
}