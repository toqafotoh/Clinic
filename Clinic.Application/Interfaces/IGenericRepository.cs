using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace Clinic.Application.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);
        Task AddAsync(T entity);
        void Update(T entity);
        void Delete(T entity);
        Task<bool> Exists(Expression<Func<T, bool>> predicate);
        Task<IEnumerable<T>> GetAllAsync(params Expression<Func<T, object>>[] includes); // new
        Task<T?> GetByIdAsync(int id, params Expression<Func<T, object>>[] includes); // new

    }
}

