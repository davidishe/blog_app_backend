using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyAppBack.Data;
using MyAppBack.Errors;

namespace MyAppBack.Controllers
{
  public class BuggsController : BaseApiController
  {
    private readonly DataDbContext _context;
    // var

    public BuggsController(DataDbContext context)
    {
      _context = context;
    }

    [HttpGet]
    [Route("notfound")]
    public ActionResult GetNotFoundRequest()
    {
      var thing = _context.Products.Find(44);
      if (thing == null)
      {
        return NotFound(new ApiResponse(404));
      }
      return Ok();
    }


    [HttpGet]
    [Route("testauth")]
    [Authorize]
    public ActionResult<string> GetSecretText()
    {
      return "secret stuff";
    }

    [HttpGet]
    [Route("servererror")]
    public ActionResult GetServerError()
    {
      var thing = _context.Products.Find(44);
      var thingToReturn = thing.ToString();

      return Ok();
    }

    [HttpGet]
    [Route("badrequest")]
    public ActionResult GetBadRequest()
    {
      return BadRequest(new ApiResponse(400));
    }

    [HttpGet]
    [Route("badrequest/{id}")]
    public ActionResult GetNotFoundRequest(int id)
    {
      return Ok();
    }


  }
}