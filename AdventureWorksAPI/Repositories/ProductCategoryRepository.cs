using Microsoft.EntityFrameworkCore.ChangeTracking;
using AdventureWorksNS.Data;
using System.Collections.Concurrent;
using System.Reflection.Metadata;
namespace AdventureWorksAPI.Repositories
{
    public class ProductCategoryRepository : IProductCategory
    {
        private static ConcurrentDictionary<int, ProductCategory>? ProductCategoryCache;
        private AdventureWorksDB db;

        public ProductCategoryRepository(AdventureWorksDB injectdDB)
        {
            
            db = injectdDB;
            if (ProductCategoryCache == null)
            {
                ProductCategoryCache = new ConcurrentDictionary<int, ProductCategory>(
                    db.ProductCategories.ToDictionary(cp => cp.ProductCategoryId));  // Debe utilizar como llave el ProductCategoryId

            }
        }
        public async Task<ProductCategory> CreateAsync(ProductCategory cp)
        {
            EntityEntry<ProductCategory> agregado = await db.ProductCategories.AddAsync(cp);
            int afectados = await db.SaveChangesAsync(); // se agrega await porque es asincrono
            if (afectados == 1)
            {
                if (ProductCategoryCache is null) return cp;
                return ProductCategoryCache.AddOrUpdate(cp.ProductCategoryId,cp, UpdateCache);
            }
            else
            {
                return null!;
            }
        }

        private ProductCategory UpdateCache(int id, ProductCategory cp)
        {
            ProductCategory? viejo;
            if (ProductCategoryCache is not null)
            {
                if (ProductCategoryCache.TryGetValue(id, out viejo))
                {
                    if (ProductCategoryCache.TryUpdate(id, cp, viejo))
                    {
                        return cp;
                    }
                }
            }
            return null!; // este es el primer metodo es el Create
        }
        public Task<IEnumerable<ProductCategory>> RetrieveAllAsync()
        {
            return Task.FromResult(ProductCategoryCache is null ?
                Enumerable.Empty<ProductCategory>() : ProductCategoryCache.Values);
        }

        public Task<ProductCategory?> RetrieveAsync(int id)
        {
            if (ProductCategoryCache is null) return null!;
            ProductCategoryCache.TryGetValue(id, out ProductCategory? cp);

            return Task.FromResult(cp);

        }

        public async Task<ProductCategory?> UpdateAsync(int id, ProductCategory cp)
        {
            db.ProductCategories.Update(cp);
            int afectados = await db.SaveChangesAsync();
            if (afectados == 1)
            {
                return UpdateCache(id, cp);
            }
            return null;
        }

        public async Task<bool?> DeleteAsync(int id)
        {
            ProductCategory? cp = db.ProductCategories.Find(id);
            if (cp is null) return false;
            db.ProductCategories.Remove(cp);
            int afectados = await db.SaveChangesAsync();
            if (afectados == 1)
            {
                if (ProductCategoryCache is null) return null;

                return ProductCategoryCache.TryRemove(id, out cp);
            }
            else
            {
                return null;
            }

        }
    }
}
