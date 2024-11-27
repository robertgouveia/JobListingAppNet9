using Devspot.Models;

namespace Devspot.Repositories;

public interface IRepository<T> where T : class
{
  Task<IEnumerable<T>> GetAllAsync(); 
  Task<JobPosting?> GetByIdAsync(int id); 
  Task AddAsync(T entity);
  Task UpdateAsync(T entity);
  Task DeleteAsync(int id);
}
