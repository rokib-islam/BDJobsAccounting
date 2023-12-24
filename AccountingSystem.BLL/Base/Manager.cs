using AccountingSystem.Abstractions.BLL.Base;
using AccountingSystem.Abstractions.Repository.Base;

namespace AccountingSystem.BLL.Base
{
    public abstract class Manager<T> : IManager<T> where T : class
    {
        private readonly IRepository<T> _repository;

        public Manager(IRepository<T> repository)
        {
            _repository = repository;
        }

        public virtual Task<bool> AddAsync(T entity)
        {
            return _repository.AddAsync(entity);
        }

        public virtual Task<ICollection<T>> GetListAsync()
        {
            return _repository.GetListAsync();
        }

        public virtual Task<bool> RemoveAsync(T entity)
        {
            return _repository.RemoveAsync(entity);
        }

        public virtual Task<bool> UpdateAsync(T entity)
        {
            return _repository.RemoveAsync(entity);
        }
    }
}