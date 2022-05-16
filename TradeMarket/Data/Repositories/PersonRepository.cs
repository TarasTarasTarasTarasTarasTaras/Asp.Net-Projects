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
    public class PersonRepository : IPersonRepository
    {
        private readonly TradeMarketDbContext db;

        public PersonRepository(TradeMarketDbContext context)
        {
            db = context;
        }

        public async Task AddAsync(Person entity)
        {
            await db.Persons.AddAsync(entity);
        }

        public async void Delete(Person entity)
        {
            db.Persons.Remove(entity);
        }

        public async Task DeleteByIdAsync(int id)
        {
            Person person = await db.Persons.FindAsync(id);
            db.Persons.Remove(person);
        }

        public async Task<IEnumerable<Person>> GetAllAsync()
        {
            return await db.Persons.ToListAsync();
        }

        public async Task<Person> GetByIdAsync(int id)
        {
            return await db.Persons.FindAsync(id);
        }

        public async void Update(Person entity)
        {
            var local = db.Set<Person>()
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
