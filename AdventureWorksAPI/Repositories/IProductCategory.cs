using AdventureWorksNS.Data;
namespace AdventureWorksAPI.Repositories
{
    public interface IProductCategory
    {
        Task<ProductCategory> CreateAsync(ProductCategory cp);
        
        Task<IEnumerable<ProductCategory>> RetrieveAllAsync(); // de todo retrive
        Task<ProductCategory?> RetrieveAsync(int id); // es solo uno por eso se puso id
        Task<ProductCategory?> UpdateAsync(int id, ProductCategory cp); // manda el id y despues se indica cual se desea reemplazar por eso la C
        Task<bool?> DeleteAsync(int id);


    }
}
