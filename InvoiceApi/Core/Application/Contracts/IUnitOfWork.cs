using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace InvoiceApi.Core.Application.Contracts
{
  public interface IUnitOfWork : IDisposable
  {
    DbContext Context { get; }
    Task<int> Commit();
  }
}
