using InvoiceApi.Core.Application.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace InvoiceApi.Infrastructure.Persistence
{
  public class AsyncUnitOfWork : IUnitOfWork
  {
    public DbContext Context { get; }

    public AsyncUnitOfWork(DbContext context)
    {
      Context = context;
    }

    public async Task<int> Commit()
    {
      return await Context.SaveChangesAsync();
    }

    public async void Dispose()
    {
      await Context.DisposeAsync();
    }
  }
}
