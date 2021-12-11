using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using InvoiceApi.Core.Application.Contracts;
using InvoiceApi.Core.Domain.Models;
using InvoiceApi.WebApi.Controllers;
using Moq;
using NUnit.Framework;

namespace UnitTests.WebApi.Controllers
{
  public class InvoiceControllerTest
  {
    private readonly Lazy<Invoice> _fakeInvoice1 = new Lazy<Invoice>(() =>
      new Invoice()
      {
        InvoiceId = "FakeInvoiceId1"
      });
    private Invoice FakeInvoice1 => _fakeInvoice1.Value;

    private readonly Lazy<Invoice> _fakeInvoice2 = new Lazy<Invoice>(() =>
      new Invoice()
      {
        InvoiceId = "FakeInvoiceId2"
      });
    private Invoice FakeInvoice2 => _fakeInvoice2.Value;

    private List<Invoice> _invoiceList;

    private Mock<IGenericRepository<Invoice>> _genericRepositoryMock;
    private Mock<IUnitOfWork> _unitOfWorkMock;
    private Mock<IExchangeService> _exchangeServiceMock;

    private InvoiceController _controller;

    [SetUp]
    protected void Setup()
    {
      _invoiceList = new List<Invoice> { FakeInvoice1, FakeInvoice2 };

      _genericRepositoryMock = InstallGenericRepositoryMock();
      _unitOfWorkMock = InstallUnitOfWorkMock();
      _exchangeServiceMock = InstallExchangeServiceMock();

      _controller = new InvoiceController(
        _genericRepositoryMock.Object,
        _unitOfWorkMock.Object,
        _exchangeServiceMock.Object);
    }

    [Test]
    public async Task GetInvoicesMethodReturnsAllInvoices()
    {
      var result = await _controller.GetInvoices(null);

      Assert.AreEqual(_invoiceList, result.Value.InvoiceList);
    }

    [Test]
    public async Task GetInvoicesMethodReturnsAllInvoicesInTheDesiredCurrency()
    {
      await _controller.GetInvoices("FakeCurrency");

      _exchangeServiceMock.Verify(x => x.Convert(FakeInvoice1, "FakeCurrency"), Times.Once());
      _exchangeServiceMock.Verify(x => x.Convert(FakeInvoice2, "FakeCurrency"), Times.Once());
    }

    [Test]
    public async Task GetInvoiceMethodReturnsTheDesiredInvoice()
    {
      var result = await _controller.GetInvoice(FakeInvoice1.InvoiceId, null);

      var resultList = result.Value.InvoiceList;
      _exchangeServiceMock.Verify(x => x.Convert(It.IsAny<Invoice>(), It.IsAny<string>()), Times.Never());
      AssertInvoicesAreEqual(FakeInvoice1, resultList.First());
    }

    [Test]
    public async Task GetInvoiceMethodConvertsTheCurrency()
    {
      var result = await _controller.GetInvoice(FakeInvoice1.InvoiceId, "USD");

      var resultList = result.Value.InvoiceList;
      _exchangeServiceMock.Verify(x => x.Convert(FakeInvoice1, "USD"), Times.Once());
      AssertInvoicesAreEqual(FakeInvoice1, resultList.First());
    }

    [Test]
    public async Task PutInvoiceMethodUpdatesTheInvoice()
    {
      var result = await _controller.PutInvoice(FakeInvoice1.InvoiceId, FakeInvoice1);

      var resultList = result.Value.InvoiceList;
      _genericRepositoryMock.Verify(x => x.Update(FakeInvoice1), Times.Once());
      _unitOfWorkMock.Verify(x => x.Commit(), Times.Once());
      AssertInvoicesAreEqual(FakeInvoice1, resultList.First());
    }

    [Test]
    public async Task PostInvoiceMethodRegistersTheInvoice()
    {
      var result = await _controller.PostInvoice(FakeInvoice1);

      var resultList = result.Value.InvoiceList;
      _genericRepositoryMock.Verify(x => x.CreateAsync(FakeInvoice1), Times.Once());
      _unitOfWorkMock.Verify(x => x.Commit(), Times.Once());
      AssertInvoicesAreEqual(FakeInvoice1, resultList.First());
    }

    [Test]
    public async Task DeleteInvoiceMethodRemovesTheInvoice()
    {
      var result = await _controller.DeleteInvoice(FakeInvoice1.InvoiceId);

      var resultList = result.Value.InvoiceList;
      _genericRepositoryMock.Verify(x => x.Remove(It.IsAny<Invoice>()), Times.Once());
      _unitOfWorkMock.Verify(x => x.Commit(), Times.Once());
      AssertInvoicesAreEqual(FakeInvoice1, resultList.First());
    }

    private Mock<IGenericRepository<Invoice>> InstallGenericRepositoryMock()
    {
      var mock = new Mock<IGenericRepository<Invoice>>();
      mock.Setup(x => x.GetAsync())
        .ReturnsAsync(_invoiceList);
      mock.Setup(x => x.GetAsync(
                          It.IsAny<Expression<Func<Invoice, bool>>>(),
                          It.IsAny<Func<IQueryable<Invoice>, IOrderedQueryable<Invoice>>>()))
        .ReturnsAsync(new List<Invoice>() { FakeInvoice1 });
      mock.Setup(x => x.CreateAsync(FakeInvoice1))
        .ReturnsAsync(FakeInvoice1);
      mock.Setup(x => x.Update(FakeInvoice1))
        .ReturnsAsync(FakeInvoice1);
      mock.Setup(x => x.Remove(FakeInvoice1))
        .ReturnsAsync(FakeInvoice1);

      return mock;
    }

    private Mock<IUnitOfWork> InstallUnitOfWorkMock()
    {
      var mock = new Mock<IUnitOfWork>();
      mock.Setup(x => x.Commit());

      return mock;
    }

    private Mock<IExchangeService> InstallExchangeServiceMock()
    {
      var mock = new Mock<IExchangeService>();
      mock.Setup(x => x.Convert(FakeInvoice1, "USD"));

      return mock;
    }

    private void AssertInvoicesAreEqual(Invoice expected, Invoice actual)
    {
      Assert.AreEqual(expected.InvoiceId, actual.InvoiceId);
      Assert.AreEqual(expected.Currency, actual.Currency);
      Assert.AreEqual(expected.DateIssued, actual.DateIssued);
      Assert.AreEqual(expected.Description, actual.Description);
      Assert.AreEqual(expected.Amount, actual.Amount);
      Assert.AreEqual(expected.Supplier, actual.Supplier);
    }
  }
}
