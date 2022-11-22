using Core.Models;
using System.Linq.Expressions;

namespace Core.Interfaces
{
    public interface IRepository<T> where T : IEntity
    {
        Task CreateAsync(T entity);
        Task<List<T>> GetAllAsync();
        Task<T> GetAsync(Expression<Func<T, bool>> filter);
        Task<T> GetAsync(Guid id);
        Task RemoveAsync(Guid id);
        Task RemoveAsync(T entity);
        Task UpdateAsync(T entity);
        Task HandleScheduleStatesAsync(IEnumerable<ScheduleStateChanged> scheduleChanges);
        Task<bool> Any();
    }
}
