using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<T> GetById(Guid Id);
        Task<IEnumerable<T>> GetAllWithExpression(Expression<Func<T, bool>> expression);
        Task<T> GetWithExpression(Expression<Func<T, bool>> expression);
        Task Add(T model);
        Task Update(T model);
        Task SaveChanges();
    }
}
