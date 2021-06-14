using Data;
using Microsoft.EntityFrameworkCore;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Services.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly Context Context;
        private readonly DbSet<T> Db;

        public Repository(Context Context)
        {
            this.Context = Context;
            Db = Context.Set<T>();
        }

        public async Task<T> GetWithExpression(Expression<Func<T, bool>> expression)
        {
            return await Db.FirstOrDefaultAsync(expression);
        }

        public async Task<IEnumerable<T>> GetAllWithExpression(Expression<Func<T, bool>> expression)
        {
            return await Db.Where(expression).ToListAsync();
        }

        public async Task Add(T model)
        {
            await Db.AddAsync(model);
            await SaveChanges();
        }

        public async Task Update(T model)
        {
            Db.Update(model);
            await SaveChanges();
        }

        public async Task<T> GetById(Guid Id)
        {
            return await Db.FindAsync(Id);
        }

        public async Task SaveChanges()
        {
            await Context.SaveChangesAsync();
        }
    }
}
