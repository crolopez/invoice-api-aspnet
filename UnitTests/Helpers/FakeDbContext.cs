using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace UnitTests.Helpers
{
  public class FakeDbContext : DbContext
  {
    private const string DatabaseName = "FakeDb";

    public DbSet<FakeModel> Data { get; set; }

    public FakeDbContext(List<FakeModel> dataList)
      : base(new DbContextOptionsBuilder().UseInMemoryDatabase(DatabaseName).Options)
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
  }
}
