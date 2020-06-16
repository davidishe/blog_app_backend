using System.ComponentModel.DataAnnotations;

namespace MyAppBack.Dtos
{
  public class BasketItemDto
  {
    [Required]
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Цена должна быть больше 1 рубля")]
    public int Price { get; set; }

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Количество должно быть больше 1 штуки")]
    public int Quantity { get; set; }

    [Required]
    public string PictureUrl { get; set; }

    [Required]
    public string Type { get; set; }

    [Required]
    public string Region { get; set; }
    [Required]
    public int GuId { get; set; }
  }
}