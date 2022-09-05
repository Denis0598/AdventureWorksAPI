using AdventureWorksNS.Data;
namespace AdventureWorksAPI.Repositories
{
    public interface ICustumerRepository
    {
        /*
         * CRUD
         * 
         * 
         */
        Task<Customer> CreateAsync(Customer c);
        Task<IEnumerable<Customer>> RetrieveAllAsync(); // de todo retrive
        Task<Customer?> RetrieveAsync(int id); // es solo uno por eso se puso id
        Task<Customer?> UpdateAsync (int id, Customer c); // manda el id y despues se indica cual se desea reemplazar por eso la C
        Task<bool?> DeleteAsync(int id);
        
    }
}
