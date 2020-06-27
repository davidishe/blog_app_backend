using System;
using System.Collections.Generic;
using MyAppBack.Models.Articles;

namespace MyAppBack.Dtos.Articles
{
  public class CommentToReturnDto
  {

    public int? Id { get; set; }
    public string CommentText { get; set; }
    public string CommentAuthorName { get; set; }
    public string CommentAuthorId { get; set; }
    public int? ArticleId { get; set; }
    public int? ParentId { get; set; }
    public virtual ICollection<CommentToReturnDto> SubComments { get; set; }
    public DateTime? EnrolledDate { get; set; }

  }
}