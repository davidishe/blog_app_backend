using System;
using Microsoft.AspNetCore.Http;

namespace MyAppBack.Dtos.User
{
  public class UserPhotoForCreationDto
  {
    public UserPhotoForCreationDto()
    {
      DateTimeAdded = DateTime.Now;
    }

    public string PhotoUrl { get; set; }
    public IFormFile File { get; set; }
    public string Description { get; set; }
    public DateTime DateTimeAdded { get; set; }
    public string PublicId { get; set; }


  }
}