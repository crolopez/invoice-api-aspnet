using InvoiceApi.Core.Application.Contracts;
using InvoiceApi.Core.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnitTests.Helpers
{
  public class FakeDbContext : DbContext
  {
    const string DatabaseName = "FakeDb";

    public DbSet<FakeModel> Data { get; set; }

    public FakeDbContext(List<FakeModel> dataList)
      : base(GetOptions())
    {
      foreach (var value in dataList)
      {
        Add(value);
      }

      SaveChanges();
    }

    public override void Dispose()
    {
      var entries = Data.ToList<FakeModel>();

      foreach (var entry in entries)
      {
        Remove(entry);
      }

      SaveChanges();
      base.Dispose();
    }

    private static DbContextOptions GetOptions()
    {
      return new DbContextOptionsBuilder()
        .UseInMemoryDatabase(DatabaseName).Options;
    }
  }
}
