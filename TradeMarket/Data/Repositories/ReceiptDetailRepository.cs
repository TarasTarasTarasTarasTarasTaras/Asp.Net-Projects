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
    public class ReceiptDetailRepository : IReceiptDetailRepository
    {
        private readonly TradeMarketDbContext db;

        public ReceiptDetailRepository(TradeMarketDbContext context)
        {
            db = context;
        }

        public async Task AddAsync(ReceiptDetail entity)
        {
            await db.ReceiptsDetails.AddAsync(entity);
        }

        public async void Delete(ReceiptDetail entity)
        {
            db.ReceiptsDetails.Remove(entity);
        }

        public async Task DeleteByIdAsync(int id)
        {
            ReceiptDetail receiptDetail = await db.ReceiptsDetails.FindAsync(id);
            db.ReceiptsDetails.Remove(receiptDetail);
        }

        public async Task<IEnumerable<ReceiptDetail>> GetAllAsync()
        {
            return await db.ReceiptsDetails.ToListAsync();
        }

        public async Task<IEnumerable<ReceiptDetail>> GetAllWithDetailsAsync()
        {
            return await
                db.ReceiptsDetails
                .Include(r => r.Product)
                .ThenInclude(r => r.Category)
                .Include(r => r.Receipt)
                .ToListAsync();
        }

        public async Task<ReceiptDetail> GetByIdAsync(int id)
        {
            return await db.ReceiptsDetails.FindAsync(id);
        }

        public async void Update(ReceiptDetail entity)
        {
            db.Entry(entity).State = EntityState.Modified;
        }
    }
}
