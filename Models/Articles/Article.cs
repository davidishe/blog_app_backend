using System.Collections.Generic;

namespace MyAppBack.Models.Articles
{
  public class Article : BaseEntity
  {
    public string Name { get; set; }
    public string? Description { get; set; }
    public string? Text { get; set; }
    public string? AuthorEmail { get; set; }
    public string? AuthorDisplayName { get; set; }
    public string? AuthorDescription { get; set; }
    public List<Comment>? Comments { get; }
    public int GuId { get; set; }
  }
}