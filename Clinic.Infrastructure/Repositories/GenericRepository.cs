using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Clinic.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Clinic.Infrastructure.Data;

namespace Clinic.Infrastructure.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly ClinicDbContext _context;
        private readonly DbSet<T> _dbSet;

        public GenericRepository(ClinicDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public async Task AddAsync(T entity) => await _dbSet.AddAsync(entity);

        public void Delete(T entity) => _dbSet.Remove(entity);

        public async Task<IEnumerable<T>> GetAllAsync() => await _dbSet.ToListAsync();

        public async Task<T> GetByIdAsync(int id) => await _dbSet.FindAsync(id);

        public void Update(T entity) => _dbSet.Update(entity);

        public async Task<bool> Exists(Expression<Func<T, bool>> predicate) =>
            await _dbSet.AnyAsync(predicate);

        public async Task<IEnumerable<T>> GetAllAsync(params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _dbSet;

            if (includes != null)
            {
                foreach (var include in includes)
                    query = query.Include(include);
            }

            return await query.ToListAsync();
        }

        public async Task<T?> GetByIdAsync(int id, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _dbSet;

            if (includes != null)
            {
                foreach (var include in includes)
                    query = query.Include(include);
            }

            // Use Find for entities without includes, otherwise use FirstOrDefaultAsync
            // This assumes your entity has a property ending with "Id" (DoctorId, PatientId, etc.)
            var keyProperty = typeof(T).GetProperties()
                .FirstOrDefault(p => p.Name.EndsWith("Id") && p.PropertyType == typeof(int));

            if (keyProperty == null)
                return await query.FirstOrDefaultAsync();

            var parameter = Expression.Parameter(typeof(T), "e");
            var property = Expression.Property(parameter, keyProperty.Name);
            var constant = Expression.Constant(id);
            var equality = Expression.Equal(property, constant);
            var lambda = Expression.Lambda<Func<T, bool>>(equality, parameter);

            return await query.FirstOrDefaultAsync(lambda);
        }
    }
}