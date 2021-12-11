using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace InvoiceApi.Core.Application.Contracts
{
  public interface IGenericRepository<T>
    where T : class
  {
    Task<IEnumerable<T>> GetAsync();
    Task<IEnumerable<T>> GetAsync(
      Expression<Func<T, bool>> whereCondition,
      Func<IQueryable<T>,
      IOrderedQueryable<T>> orderBy);
    Task<T> CreateAsync(T entity);
    Task<T> Update(T entity);
    Task<T> Remove(T entity);
  }
}
