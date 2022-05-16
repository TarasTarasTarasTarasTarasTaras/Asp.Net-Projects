using Data.Data;
using Data.Entities;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class ProductCategoryRepository : IProductCategoryRepository
    {
        private readonly TradeMarketDbContext db;

        public ProductCategoryRepository(TradeMarketDbContext context)
        {
            db = context;
        }

        public async Task AddAsync(ProductCategory entity)
        {
            await db.ProductCategories.AddAsync(entity);
            await db.SaveChangesAsync();
        }

        public async void Delete(ProductCategory entity)
        {
            db.ProductCategories.Remove(entity);
            await db.SaveChangesAsync();
        }

        public async Task DeleteByIdAsync(int id)
        {
            ProductCategory productCategory = await db.ProductCategories.FindAsync(id);
            db.ProductCategories.Remove(productCategory);
            await db.SaveChangesAsync();
        }

        public async Task<IEnumerable<ProductCategory>> GetAllAsync()
        {
            return await db.ProductCategories.ToListAsync();
        }

        public async Task<ProductCategory> GetByIdAsync(int id)
        {
            return await db.ProductCategories.FindAsync(id);
        }

        public async void Update(ProductCategory entity)
        {
            db.Entry(entity).State = EntityState.Modified;
            await db.SaveChangesAsync();
        }
    }
}
