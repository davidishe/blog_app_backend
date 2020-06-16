using System;
using System.Collections.Generic;
using MyAppBack.Models.Articles;

namespace MyAppBack.Dtos.Articles
{
  public class ArticleToReturnDto
  {
    public string Name { get; set; }
    public string Description { get; set; }
    public string? Text { get; set; }
    public string? AuthorEmail { get; set; }
    public string? AuthorDisplayName { get; set; }
    public string? AuthorDescription { get; set; }
    public List<Comment>? Comments { get; set; }
    public int GuId { get; set; }
  }
}

// public DateTime? EnrolledDate { get; set; } = DateTime.Now;
