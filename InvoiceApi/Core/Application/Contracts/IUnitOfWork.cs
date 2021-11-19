using System;
using Microsoft.EntityFrameworkCore;

namespace InvoiceApi.Core.Application.Contracts
{
  public interface IUnitOfWork : IDisposable
  {
    DbContext Context { get; }
    void Commit();
  }
}
