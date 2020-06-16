using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyAppBack.Data.Config;
using MyAppBack.Data.Spec;
using MyAppBack.Models;

namespace MyAppBack.Data.Repos.GenericRepository
{
  public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
  {

    private readonly DataDbContext _context;
    public GenericRepository(DataDbContext context)
    {
      _context = context;
    }


    // generic
    public async Task<IReadOnlyList<T>> ListAllAsync()
    {
      return await _context.Set<T>().ToListAsync();
    }

    public async Task<T> GetByIdAsync(int id)
    {
      return await _context.Set<T>().FindAsync(id);
    }

    public async Task<Product> GetByGuIdAsync(int guiId)
    {
      return await _context.Products.Where(x => x.GuId == guiId).FirstOrDefaultAsync();
    }


    public async Task<T> GetEntityWithSpec(ISpecification<T> spec)
    {
      return await ApplySpecification(spec).FirstOrDefaultAsync();
    }

    public async Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec)
    {
      return await ApplySpecification(spec).ToListAsync();
    }



    public void Add(T entity)
    {
      _context.Set<T>().Add(entity);
    }

    public void Update(T entity)
    {
      _context.Set<T>().Attach(entity);
      _context.Entry(entity).State = EntityState.Modified;
    }

    void IGenericRepository<T>.Delete(T entity)
    {
      _context.Set<T>().Remove(entity);
    }


    public async Task<IReadOnlyList<ProductType>> CreateProductTypeAsync(ProductType productType)
    {
      var types = _context.ProductTypes;
      _context.Add(productType);
      _context.SaveChanges();
      return await _context.ProductTypes.ToListAsync();
    }

    public async Task<IReadOnlyList<ProductRegion>> CreateProductRegionAsync(ProductRegion productRegion)
    {
      var types = _context.ProductRegions;
      _context.Add(productRegion);
      _context.SaveChanges();
      return await _context.ProductRegions.ToListAsync();
    }

    public async Task<int> CountAsync(ISpecification<T> spec)
    {
      return await ApplySpecification(spec).CountAsync();
    }

    private IQueryable<T> ApplySpecification(ISpecification<T> spec)
    {
      return SpecificationEvaluator<T>.GetQuery(_context.Set<T>().AsQueryable(), spec);
    }



    // end generic








    // public async Task<IReadOnlyList<Product>> Add(Product entity)
    // {
    //   _context.Add(entity);
    //   _context.SaveChanges();
    //   return await _context.Products.Include(x => x.ProductType).Include(x => x.ProductRegion).ToListAsync();
    // }
    // public async Task<bool> SaveAll()
    // {
    //   return await _context.SaveChangesAsync() > 0;
    // }

    // public async Task<PagedList<Product>> GetProducts(UserParams userParams)
    // {
    //   var products = _context.Products;
    //   return await PagedList<Product>.CreateAsync(products, userParams.PageIndex, userParams.PageSize);
    // }

    // public async Task<IReadOnlyList<ProductType>> GetProductTypesAsync()
    // {
    //   return await _context.ProductTypes.ToListAsync();
    // }

    // public async Task<IReadOnlyList<ProductRegion>> GetProductRegionsAsync()
    // {
    //   return await _context.ProductRegions.ToListAsync();
    // }
    // public async Task<IReadOnlyList<Product>> Delete(T entity)
    // {
    //   _context.Remove(entity);
    //   _context.SaveChanges();
    //   return await _context.Products.Include(x => x.ProductType).Include(x => x.ProductRegion).ToListAsync();
    // }


  }
}