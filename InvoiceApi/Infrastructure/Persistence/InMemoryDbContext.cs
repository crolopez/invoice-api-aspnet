using InvoiceApi.Core.Application.Contracts;
using InvoiceApi.Core.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace InvoiceApi.Infrastructure.Persistence
{
  public class InMemoryDbContext : DbContext
  {
    const string DatabaseName = "Invoices";

    public DbSet<Invoice> Invoices { get; set; }

    public InMemoryDbContext()
      : base(new DbContextOptionsBuilder().UseInMemoryDatabase(DatabaseName).Options)
    {
    }
  }
}
