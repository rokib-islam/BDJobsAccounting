using AccountingSystem.Abstractions.Repository.Base;
using Microsoft.EntityFrameworkCore;

namespace AccountingSystem.Repository.EFBase
{
    public abstract class EfRepository<T> : IRepository<T> where T : class
    {
        private readonly DbContext _context;

        public EfRepository(DbContext context)
        {
            _context = context;
        }

        public virtual async Task<bool> AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            return await _context.SaveChangesAsync() > 0;
        }

        public virtual async Task<T> FindIdAsync(int entity)
        {
            return await _context.Set<T>().FindAsync(entity);
        }

        public virtual async Task<ICollection<T>> GetListAsync()
        {
            return await _context.Set<T>().AsNoTracking().ToListAsync();
        }

        public virtual async Task<bool> RemoveAsync(T entity)
        {
            _context.Set<T>().Remove(entity);
            return await _context.SaveChangesAsync() > 0;
        }

        public virtual async Task<bool> UpdateAsync(T entity)
        {
            _context.Set<T>().Update(entity);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}