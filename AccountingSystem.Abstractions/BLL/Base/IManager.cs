namespace AccountingSystem.Abstractions.BLL.Base
{
    public interface IManager<T> where T : class
    {
        Task<bool> AddAsync(T entity);

        Task<bool> UpdateAsync(T entity);

        Task<bool> RemoveAsync(T entity);

        Task<ICollection<T>> GetListAsync();
    }
}