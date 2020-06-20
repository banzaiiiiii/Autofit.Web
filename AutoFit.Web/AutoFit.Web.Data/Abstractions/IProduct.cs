using System.Collections.Generic;
using System.Threading.Tasks;

namespace AutoFit.Web.Data.Abstractions
{
    public interface IProduct
    {
        Task Add(string name, string description, string value);
      
        Task Delete(int id);

        Task<Product> Update(Product product);

        Task<Product> GetProduct(string name);
        Task<List<Product>> GetProducts();

    }
}
