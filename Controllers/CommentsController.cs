using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyAppBack.Data;
using MyAppBack.Dtos.Articles;
using MyAppBack.Models.Articles;



namespace MyAppBack.Controllers
{
  public class CommentsController : BaseApiController
  {
    private readonly DataDbContext _context;
    private readonly IMapper _mapper;

    public CommentsController(DataDbContext context, IMapper mapper)
    {
      _context = context;
      _mapper = mapper;
    }

    [AllowAnonymous]
    [HttpPost]
    [Route("post")]
    public IActionResult Post(CommentDto comment)
    {
      try
      {
        Comment nodeDb = new Comment();
        if (comment.ParentId == -1)
        {
          nodeDb.CommentText = comment.CommentText;
          _context.Comments.Add(nodeDb);
          _context.SaveChanges();
        }
        else
        {
          Comment parentNodeDb = _context.Comments.Include(x => x.SubComments).Where(z => z.Id == comment.ParentId).FirstOrDefault();
          nodeDb.CommentText = comment.CommentText;
          parentNodeDb.SubComments.Add(nodeDb);
          _context.SaveChanges();
        }
        return Ok(nodeDb);
      }
      catch (Exception ex)
      {
        return new JsonResult(new { error = ex.Message });
      }

    }



    [AllowAnonymous]
    [HttpGet]
    [Route("all")]
    public IActionResult GetAll()
    {

      var comments = _context.Comments.ToList();
      return Ok(comments);

    }

    [AllowAnonymous]
    [HttpGet]
    [Route("ranged")]
    public IActionResult GetRanged()
    {
      IQueryable<Comment> comments = _context.Comments;
      var commentsToReturn = _mapper.Map<IReadOnlyList<Comment>, IReadOnlyList<CommentToReturnDto>>(comments.ToList());

      return Ok(commentsToReturn.Where(z => z.ParentId == null));
      // return Ok(commentsToReturn);

    }

    private IQueryable<Comment> Calculate(IQueryable<Comment> comments)
    {
      return comments.Include(z => z.SubComments).ThenInclude(z => z.SubComments);
    }
  }

}
