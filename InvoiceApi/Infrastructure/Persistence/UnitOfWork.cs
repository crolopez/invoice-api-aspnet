using InvoiceApi.Core.Application.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace InvoiceApi.Infrastructure.Persistence
{
  public class AsyncUnitOfWork : IUnitOfWork
  {
    public DbContext Context { get; }

    public AsyncUnitOfWork(DbContext context)
    {
      Context = context;
    }

    public async void Commit()
    {
      await Context.SaveChangesAsync();
    }

    public async void Dispose()
    {
      await Context.DisposeAsync();
    }
  }
}
