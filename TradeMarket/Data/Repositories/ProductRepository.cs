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
    public class ProductRepository : IProductRepository
    {
        private readonly TradeMarketDbContext db;

        public ProductRepository(TradeMarketDbContext context)
        {
            db = context;
        }

        public async Task AddAsync(Product entity)
        {
            await db.Products.AddAsync(entity);
            await db.SaveChangesAsync();
        }

        public async void Delete(Product entity)
        {
            db.Products.Remove(entity);
            await db.SaveChangesAsync();
        }

        public async Task DeleteByIdAsync(int id)
        {
            Product product = await db.Products.FindAsync(id);
            db.Products.Remove(product);
            await db.SaveChangesAsync();
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await db.Products.ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetAllWithDetailsAsync()
        {
            return await
                db.Products
                .Include(p => p.Category)
                .Include(p => p.ReceiptDetails)
                .ToListAsync();
        }

        public async Task<Product> GetByIdAsync(int id)
        {
            return await db.Products.FindAsync(id);
        }

        public async Task<Product> GetByIdWithDetailsAsync(int id)
        {
            return await
                db.Products
                .Include(p => p.Category)
                .Include(p => p.ReceiptDetails)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async void Update(Product entity)
        {
            db.Entry(entity).State = EntityState.Modified;
            await db.SaveChangesAsync();
        }
    }
}
