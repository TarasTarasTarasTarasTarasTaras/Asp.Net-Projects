using Data.Data;
using Data.Entities;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class ReceiptRepository : IReceiptRepository
    {
        private readonly TradeMarketDbContext db;

        public ReceiptRepository(TradeMarketDbContext context)
        {
            db = context;
        }

        public async Task AddAsync(Receipt entity)
        {
            await db.Receipts.AddAsync(entity);
        }

        public async void Delete(Receipt entity)
        {
            db.Receipts.Remove(entity);
        }

        public async Task DeleteByIdAsync(int id)
        {
            Receipt receipt = await db.Receipts.FindAsync(id);
            db.Receipts.Remove(receipt);
        }

        public async Task<IEnumerable<Receipt>> GetAllAsync()
        {
            return await db.Receipts.ToListAsync();
        }

        public async Task<IEnumerable<Receipt>> GetAllWithDetailsAsync()
        {
            return await
                db.Receipts
                .Include(r => r.Customer)
                .Include(r => r.ReceiptDetails)
                .ThenInclude(r => r.Product)
                .ThenInclude(r => r.Category)
                .ToListAsync();
        }

        public async Task<Receipt> GetByIdAsync(int id)
        {
            return await db.Receipts.FindAsync(id);
        }

        public async Task<Receipt> GetByIdWithDetailsAsync(int id)
        {
            return await
                db.Receipts
                .Include(r => r.Customer)
                .Include(r => r.ReceiptDetails)
                .ThenInclude(r => r.Product)
                .ThenInclude(r => r.Category)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async void Update(Receipt entity)
        {
            var local = db.Set<Receipt>()
                .Local
                .FirstOrDefault(entry => entry.Id.Equals(entity.Id));

            // check if local is not null 
            if (local != null)
            {
                // detach
                db.Entry(local).State = EntityState.Detached;
            }

            db.Entry(entity).State = EntityState.Modified;
        }
    }
}
