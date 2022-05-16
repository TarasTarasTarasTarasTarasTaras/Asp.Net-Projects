using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Data;
using Data.Entities;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly TradeMarketDbContext db;

        public CustomerRepository(TradeMarketDbContext context)
        {
            db = context;
        }

        public async Task AddAsync(Customer entity)
        {
            await db.Customers.AddAsync(entity);
        }

        public async void Delete(Customer entity)
        {
            db.Customers.Remove(entity);
        }

        public async Task DeleteByIdAsync(int id)
        {
            Customer customer = await db.Customers.FindAsync(id);
            db.Customers.Remove(customer);
        }

        public async Task<IEnumerable<Customer>> GetAllAsync()
        {
            return await db.Customers.ToListAsync();
        }

        public async Task<IEnumerable<Customer>> GetAllWithDetailsAsync()
        {
            return await 
                db.Customers
                .Include(c => c.Person)
                .Include(c => c.Receipts)
                .ThenInclude(c => c.ReceiptDetails)
                .ToListAsync();
        }

        public async Task<Customer> GetByIdAsync(int id)
        {
            return await db.Customers.FindAsync(id);
        }

        public async Task<Customer> GetByIdWithDetailsAsync(int id)
        {
            return await
                db.Customers
                .Include(c => c.Person)
                .Include(c => c.Receipts)
                .ThenInclude(c => c.ReceiptDetails)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async void Update(Customer entity)
        {
            var local = db.Set<Customer>()
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
