using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyAppBack.Controllers;
using MyAppBack.Data.Repos.GenericRepository;
using MyAppBack.Data.Spec.Articles;
using MyAppBack.Dtos.Articles;
using MyAppBack.Helpers;
using MyAppBack.Models.Articles;

namespace MyAppBack.Controllers
{
  public class ArticlesController : BaseApiController
  {

    private readonly IGenericRepository<Article> _articlesRepo;
    private readonly IMapper _mapper;


    public ArticlesController(IGenericRepository<Article> articlesRepo, IMapper mapper)
    {
      _articlesRepo = articlesRepo;
      _mapper = mapper;
    }

    [AllowAnonymous]
    [HttpGet]
    [Route("all")]
    public async Task<ActionResult<PagedList<ArticleToReturnDto>>> GetArticles([FromQuery] UserParams? userParams)
    {
      var spec = new ArticlesSpecification(userParams);
      var totalItems = await _articlesRepo.CountAsync(spec);

      var article = await _articlesRepo.ListAsync(spec);
      var data = _mapper.Map<IReadOnlyList<Article>, IReadOnlyList<ArticleToReturnDto>>(article);
      await SetTimeOut();
      return Ok(new Pagination<ArticleToReturnDto>(userParams.PageIndex, userParams.PageSize, totalItems, data));
    }

    [AllowAnonymous]
    [HttpGet("{id}")]
    [Route("article")]
    public async Task<ActionResult<ArticleToReturnDto>> GetArticleByIdAsync([FromQuery] int id)
    {
      var spec = new ArticlesSpecification(id);
      var article = await _articlesRepo.GetEntityWithSpec(spec);

      await SetTimeOut();
      var articleToReturn = _mapper.Map<Article, ArticleToReturnDto>(article);
      var comments = articleToReturn.Comments.Where(z => z.ParentId == null).ToList();
      articleToReturn.Comments = comments;
      return Ok(articleToReturn);

    }


    private async Task<bool> SetTimeOut()
    {
      await Task.Delay(0);
      return true;
    }
  }
}