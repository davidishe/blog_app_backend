using System;
using System.Collections.Generic;
using MyAppBack.Models.Articles;

namespace MyAppBack.Dtos.Articles
{
  public class ArticleToReturnDto
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string? Text { get; set; }
    public string? AuthorEmail { get; set; }
    public string? AuthorDisplayName { get; set; }
    public string? AuthorDescription { get; set; }
    public string? AuthorPhoto { get; set; }
    public List<CommentToReturnDto>? Comments { get; set; }
    public int GuId { get; set; }
  }
}

