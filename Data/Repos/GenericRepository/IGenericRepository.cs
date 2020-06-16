using System.Collections.Generic;
using System.Threading.Tasks;
using MyAppBack.Data.Spec;
using MyAppBack.Helpers;
using MyAppBack.Models;

namespace MyAppBack.Data.Repos.GenericRepository
{
  public interface IGenericRepository<T> where T : BaseEntity
  {
    Task<T> GetByIdAsync(int id);
    Task<IReadOnlyList<T>> ListAllAsync();
    Task<T> GetEntityWithSpec(ISpecification<T> spec);
    Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec);
    Task<int> CountAsync(ISpecification<T> spec);
    Task<Product> GetByGuIdAsync(int guiId);


    void Add(T entity);
    void Update(T entity);
    void Delete(T entity);




    Task<IReadOnlyList<ProductRegion>> CreateProductRegionAsync(ProductRegion productRegion);
    // Task<IReadOnlyList<Product>> Add(Product entity);
    // Task<bool> SaveAll();
    // Task<IReadOnlyList<Product>> DeleteProduct(T entity);
    // Task<Product> GetProductByIdAsync(int id);
    // Task<PagedList<Product>> GetProducts(UserParams userParams);
    // Task<IReadOnlyList<ProductType>> GetProductTypesAsync();
    Task<IReadOnlyList<ProductType>> CreateProductTypeAsync(ProductType productType);
    // Task<IReadOnlyList<ProductRegion>> GetProductRegionsAsync();
  }
}