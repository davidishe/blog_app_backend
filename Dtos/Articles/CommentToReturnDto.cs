using System.Collections.Generic;
using MyAppBack.Models.Articles;

namespace MyAppBack.Dtos.Articles
{
  public class CommentToReturnDto
  {

    public int? Id { get; set; }
    public string CommentText { get; set; }
    public int? ParentId { get; set; }
    // public virtual NodeToReturnDto ParentNode { get; set; }
    public virtual ICollection<CommentToReturnDto> SubComments { get; set; }
    // public DateTime? EnrolledDate { get; set; } = DateTime.Now;

  }
}