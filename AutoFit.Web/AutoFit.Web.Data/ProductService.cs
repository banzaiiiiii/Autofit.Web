using AutoFit.Web.Data.Abstractions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AutoFit.Web.Data
{
    public class ProductService : IProduct
    {
        private WebsiteDbContext _dbContext;

        public ProductService(WebsiteDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task Add(string name, string description, string value)
        {     
            
            _dbContext.Add(new Product
            {
               
                Name = name,
                Description = description,
                Value = value
            });
           await _dbContext.SaveChangesAsync();
        }

        public async Task<List<Product>> GetProducts()
        {
            return await _dbContext.Products.ToListAsync();
        }

        public async Task<Product> GetProduct(string name)
        {
            return await _dbContext.Products.FromSql($"SELECT * FROM dbo.Products WHERE Name = {name}").FirstOrDefaultAsync();
        }


        public async Task<Product> Update(Product product)
        {
            _dbContext.Attach(product).State = EntityState.Modified;
           await _dbContext.SaveChangesAsync();
            return product;
        }

        public async Task Delete(int id)
        {
            var car = _dbContext.Products.Find(id);
            _dbContext.Products.Remove(car);
           await _dbContext.SaveChangesAsync();
        }
        
    }
}
