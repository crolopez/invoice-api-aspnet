using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using InvoiceApi.Core.Application.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace InvoiceApi.Infrastructure.Persistence
{
  public class GenericRepository<T> : IGenericRepository<T>
    where T : class
  {
    private readonly IUnitOfWork _unitOfWork;
    public GenericRepository(IUnitOfWork unitOfWork)
    {
      _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<T>> GetAsync()
    {
      return await _unitOfWork.Context.Set<T>().ToListAsync();
    }

    #nullable enable
    public async Task<IEnumerable<T>> GetAsync(
      Expression<Func<T, bool>>? whereCondition,
      Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy)
    {
      IQueryable<T> query = _unitOfWork.Context.Set<T>();

      if (whereCondition != null)
      {
          query = query.Where(whereCondition);
      }

      return orderBy != null
        ? await orderBy(query).ToListAsync()
        : await query.ToListAsync();
    }
    #nullable disable

    public async Task<T> CreateAsync(T entity)
    {
      EntityEntry<T> entry = await _unitOfWork.Context.Set<T>().AddAsync(entity);
      return entry.Entity;
    }

    public Task<T> Update(T entity)
    {
      EntityEntry<T> entry = _unitOfWork.Context.Set<T>().Update(entity);
      return Task.FromResult(entry.Entity);
    }

    public Task<T> Remove(T entity)
    {
      EntityEntry<T> entry = _unitOfWork.Context.Set<T>().Remove(entity);
      return Task.FromResult(entry.Entity);
    }
  }
}
