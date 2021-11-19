using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace InvoiceApi.Core.Application.Contracts
{
  public interface IGenericRepository<T> where T : class
  {
    Task<IEnumerable<T>> GetAsync();
    Task<IEnumerable<T>> GetAsync(Expression<Func<T, bool>> whereCondition = null,
                        Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null);
    Task<T> CreateAsync(T entity);
    T Update(T entity);
    T Remove(T entity);
  }
}
