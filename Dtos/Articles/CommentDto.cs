using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyAppBack.Dtos.Articles
{
  public class CommentDto
  {

    public CommentDto()
    {
      Id = -1;
      ParentId = -1;
    }


    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int? Id { get; set; }

    [Required(ErrorMessage = "Комментарий не должен быть пустым")]
    public string CommentText { get; set; }
    public int? ParentId { get; set; }

  }
}
