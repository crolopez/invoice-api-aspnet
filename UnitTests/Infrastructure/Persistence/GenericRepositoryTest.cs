using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InvoiceApi.Core.Application.Contracts;
using InvoiceApi.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using UnitTests.Helpers;

namespace UnitTests.WebApi.Controllers
{
  public class GenericRepositoryTest
  {
    private Mock<IUnitOfWork> _unitOfWorkMock;

    private FakeDbContext _fakeDbContext;

    private GenericRepository<FakeModel> _genericRepository;

    private List<FakeModel> _dataList;

    [SetUp]
    protected void Setup()
    {
      _dataList = new List<FakeModel>()
      {
        new FakeModel("First", 1),
        new FakeModel("Second", 2),
        new FakeModel("Third", 3)
      };

      _fakeDbContext = new FakeDbContext(_dataList);
      _unitOfWorkMock = InstallUnitOfWorkMock();

      _genericRepository = new GenericRepository<FakeModel>(_unitOfWorkMock.Object);
    }

    [TearDown]
    protected void CleanUp()
    {
      _fakeDbContext.Dispose();
    }

    [Test]
    public async Task GetAsyncMethodReturnsAllEntries()
    {
      var entries = await _genericRepository.GetAsync();

      Assert.AreEqual(_dataList.Count, entries.Count());
      Assert.False(_dataList.Any(x => !entries.Contains(x)));
    }

    [Test]
    public async Task GetAsyncMethodFiltersCorrectly()
    {
      var entries = await _genericRepository.GetAsync(x => x.Id == "Second", null);

      Assert.AreEqual(1, entries.Count());
      Assert.AreEqual("Second", entries.First().Id);
    }

    [Test]
    public async Task CreateAsyncMethodReturnsTheCreatedEntry()
    {
      var newEntry = new FakeModel("New entry", 0);
      var entry = await _genericRepository.CreateAsync(newEntry);

      Assert.AreEqual(newEntry, entry);
    }

    [Test]
    public async Task CreateAsyncMethodAddsAnEntryIfContextIsSaved()
    {
      await _genericRepository.CreateAsync(
        new FakeModel("New entry", 0)
      );
      _fakeDbContext.SaveChanges();

      var values = await _genericRepository.GetAsync(x => x.Id == "New entry", null);
      Assert.AreEqual(1, values.Count());
      Assert.AreEqual("New entry", values.First().Id);
    }

    [Test]
    public async Task CreateAsyncMethodDoesntAddAnEntryIfContextIsNotSaved()
    {
      await _genericRepository.CreateAsync(
        new FakeModel("New entry", 0)
      );

      var values = await _genericRepository.GetAsync(x => x.Id == "New entry", null);
      Assert.AreEqual(0, values.Count());
    }

    [Test]
    public async Task UpdateMethodUpdatesAnEntryIfContextIsSaved()
    {
      _dataList[0].Value = 100;
      _genericRepository.Update(_dataList[0]);
      _fakeDbContext.SaveChanges();

      var values = await _genericRepository.GetAsync(x => x.Id == "First", null);
      Assert.AreEqual(100, values.First().Value);
    }

    [Test]
    public async Task RemoveMethodRemovesAnEntryIfContextIsSaved()
    {
      _genericRepository.Remove(_dataList[0]);
      _fakeDbContext.SaveChanges();

      var values = await _genericRepository.GetAsync(x => x.Id == "First", null);
      Assert.AreEqual(0, values.Count());
    }

    [Test]
    public async Task RemoveMethodDoesntRemoveAnEntryIfContextIsNotSaved()
    {
      _genericRepository.Remove(_dataList[0]);

      var values = await _genericRepository.GetAsync(x => x.Id == "First", null);
      Assert.AreEqual(1, values.Count());
    }

    private Mock<IUnitOfWork> InstallUnitOfWorkMock()
    {
      var mock = new Mock<IUnitOfWork>();
      mock.Setup(x => x.Context)
        .Returns(_fakeDbContext);

      return mock;
    }
  }
}
