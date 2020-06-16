using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MyAppBack.Data.Repos;
using MyAppBack.Data.Repos.GenericRepository;
using MyAppBack.Data.Spec;
using MyAppBack.Dtos;
using MyAppBack.Helpers;
using MyAppBack.Models;

namespace MyAppBack.Controllers
{

  public class ProductsController : BaseApiController
  {

    private readonly IGenericRepository<Product> _productsRepo;
    private readonly IGenericRepository<ProductType> _productTypeRepo;
    private readonly IGenericRepository<ProductRegion> _productRegionRepo;
    private readonly IMapper _mapper;

    public ProductsController(IGenericRepository<Product> productsRepo, IGenericRepository<ProductType> productTypeRepo, IGenericRepository<ProductRegion> productRegionRepo, IMapper mapper)
    {
      _productsRepo = productsRepo;
      _productTypeRepo = productTypeRepo;
      _productRegionRepo = productRegionRepo;
      _mapper = mapper;
    }

    // [Cached(600)]
    [AllowAnonymous]
    [HttpGet]
    [Route("all")]
    public async Task<ActionResult<Pagination<ProductToReturnDto>>> GetDtoProducts([FromQuery] UserParams userParams)
    {
      var spec = new ProductsWithTypesAndRegionsSpecification(userParams);
      var specForCount = new ProductWithFiltersForCountSpecification(userParams);
      var totalItems = await _productsRepo.CountAsync(specForCount);
      var products = await _productsRepo.ListAsync(spec);
      var data = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(products);
      await SetTimeOut();
      return Ok(new Pagination<ProductToReturnDto>(userParams.PageIndex, userParams.PageSize, totalItems, data));
    }

    // [Cached(10)]
    [AllowAnonymous]
    [HttpGet("{id}")]
    [Route("product")]
    public async Task<ActionResult<ProductToReturnDto>> GetProductByIdAsync([FromQuery] int id)
    {
      var spec = new ProductsWithTypesAndRegionsSpecification(id);
      var product = await _productsRepo.GetEntityWithSpec(spec);
      await SetTimeOut();
      return _mapper.Map<Product, ProductToReturnDto>(product);

    }

    [AllowAnonymous]
    [HttpGet("{guId}")]
    [Route("getproductid")]
    public async Task<ActionResult<ProductToReturnDto>> GetProductIdByGuIdAsync([FromQuery] int guId)
    {
      var product = await _productsRepo.GetByGuIdAsync(guId);
      await SetTimeOut();
      return _mapper.Map<Product, ProductToReturnDto>(product);
    }


    // [Cached(600)]
    [AllowAnonymous]
    [HttpGet]
    [Route("types")]
    public async Task<ActionResult<IReadOnlyList<ProductType>>> GetProductTypesByIdAsync()
    {
      var product = await _productTypeRepo.ListAllAsync();
      await SetTimeOut();
      return Ok(product);
    }

    [AllowAnonymous]
    [HttpPost]
    [Route("create-type")]
    public async Task<ActionResult<IReadOnlyList<ProductType>>> CreateProductTypeAsync(ProductType productType)
    {
      var product = await _productsRepo.CreateProductTypeAsync(productType);
      await SetTimeOut();
      return Ok(product);
    }

    // [Cached(600)]
    [AllowAnonymous]
    [HttpGet]
    [Route("regions")]
    public async Task<ActionResult<IReadOnlyList<ProductType>>> GetProductRegionsByIdAsync()
    {
      var product = await _productRegionRepo.ListAllAsync();
      await SetTimeOut();
      return Ok(product);
    }

    [AllowAnonymous]
    [HttpPost]
    [Route("create-region")]
    public async Task<ActionResult<IReadOnlyList<ProductType>>> CreateProductRegionAsync(ProductRegion productRegion)
    {
      var product = await _productsRepo.CreateProductRegionAsync(productRegion);
      await SetTimeOut();
      return Ok(product);
    }

    private async Task<bool> SetTimeOut()
    {
      await Task.Delay(0);
      return true;
    }

  }
}