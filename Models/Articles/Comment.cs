using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyAppBack.Models.Articles
{
  public class Comment
  {
    public Comment()
    {
      SubComments = new HashSet<Comment>();
    }

    // здесь не хватает конструктора!!!

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int? Id { get; set; }
    public int? ArticleId { get; set; }

    public Article? Article { get; set; }

    [Required(ErrorMessage = "Комментарий не должен быть пустым")]
    public string CommentText { get; set; }
    public int? ParentId { get; set; }
    public virtual Comment? ParentComment { get; set; }
    public ICollection<Comment>? SubComments { get; set; }
    public DateTime EnrolledDate { get; set; } = DateTime.Now;
  }
}