namespace AccountingSystem.Abstractions.Repository.Base
{
    public interface IRepository<T> where T : class
    {
        Task<bool> AddAsync(T entity);

        Task<bool> UpdateAsync(T entity);

        Task<bool> RemoveAsync(T entity);

        Task<ICollection<T>> GetListAsync();
    }
}