using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyAppBack.Data;
using MyAppBack.Dtos.Articles;
using MyAppBack.Extensionss;
using MyAppBack.Identity;
using MyAppBack.Models.Articles;



namespace MyAppBack.Controllers
{
  public class CommentsController : BaseApiController
  {
    private readonly DataDbContext _context;
    private readonly UserManager<AppUser> _userManager;
    private readonly IMapper _mapper;

    public CommentsController(UserManager<AppUser> userManager, DataDbContext context, IMapper mapper)
    {
      _context = context;
      _mapper = mapper;
      _userManager = userManager;
    }

    [AllowAnonymous]
    [HttpPost]
    [Route("post")]
    public async Task<ActionResult> Post(CommentDto comment)
    {
      var user = await _userManager.FindByClaimsCurrentUser(HttpContext.User);
      try
      {
        Comment nodeDb = new Comment();
        if (comment.ParentId == -1)
        {
          nodeDb.CommentText = comment.CommentText;
          nodeDb.ArticleId = comment.ArticleId;
          nodeDb.CommentAuthorName = user.DisplayName;
          nodeDb.CommentAuthorId = user.Id;


          _context.Comments.Add(nodeDb);
          _context.SaveChanges();
        }
        else
        {
          Comment parentNodeDb = _context.Comments.Include(x => x.SubComments).Where(z => z.Id == comment.ParentId).FirstOrDefault();
          nodeDb.CommentText = comment.CommentText;
          nodeDb.ArticleId = comment.ArticleId;
          nodeDb.ParentId = comment.ParentId;
          nodeDb.CommentAuthorName = user.DisplayName;
          nodeDb.CommentAuthorId = user.Id;

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
    [HttpPut]
    [Route("edit")]
    public async Task<ActionResult> Edit(string text, int commentId)
    {
      var user = await _userManager.FindByClaimsCurrentUser(HttpContext.User);
      var comment = _context.Comments.Where(x => x.Id == commentId).FirstOrDefault();
      comment.CommentText = text;
      try
      {
        _context.Comments.Update(comment);
        _context.SaveChanges();
        return Ok(comment);
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

    }





    private IQueryable<Comment> Calculate(IQueryable<Comment> comments)
    {
      return comments.Include(z => z.SubComments).ThenInclude(z => z.SubComments);
    }
  }

}
