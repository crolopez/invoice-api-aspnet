using System;
using NUnit.Framework;
using Moq;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using InvoiceApi.Core.Domain.Models;
using InvoiceApi.Core.Application.Contracts;
using InvoiceApi.WebApi.Controllers;
using System.Linq.Expressions;

namespace UnitTests.WebApi.Controllers
{
  public class InvoiceControllerTest
  {
    #region Common variables

    private static readonly Lazy<Invoice> _fakeInvoice1 = new Lazy<Invoice>(() =>
      new Invoice()
      {
        InvoiceId = "FakeInvoiceId1"
      });
    private static Invoice FakeInvoice1 => _fakeInvoice1.Value;

    private static readonly Lazy<Invoice> _fakeInvoice2 = new Lazy<Invoice>(() =>
      new Invoice()
      {
        InvoiceId = "FakeInvoiceId2"
      });
    private static Invoice FakeInvoice2 => _fakeInvoice2.Value;

    private readonly Lazy<List<Invoice>> _invoiceList = new Lazy<List<Invoice>>(() =>
      new List<Invoice>{ FakeInvoice1, FakeInvoice2 });
    private List<Invoice> InvoiceList => _invoiceList.Value;

    private Mock<IGenericRepository<Invoice>> _genericRepositoryMock;
    private Mock<IUnitOfWork> _unitOfWorkMock;
    private Mock<IExchangeService> _exchangeServiceMock;

    private InvoiceController _controller;

    #endregion

    #region Test Methods

    [SetUp]
    protected void Setup()
    {
      _genericRepositoryMock = InstallGenericRepositoryMock();
      _unitOfWorkMock = InstallUnitOfWorkMock();
      _exchangeServiceMock = InstallExchangeServiceMock();

      _controller = new InvoiceController(_genericRepositoryMock.Object,
                                          _unitOfWorkMock.Object,
                                          _exchangeServiceMock.Object);
    }

    [Test]
    public async Task GetInvoicesMethodReturnsAllInvoices()
    {
      var result = await _controller.GetInvoices();

      Assert.AreEqual(InvoiceList, result.Value);
    }

    [Test]
    public async Task GetInvoiceMethodReturnsTheDesiredInvoice()
    {
      var result = await _controller.GetInvoice(FakeInvoice1.InvoiceId, null);

      _exchangeServiceMock.Verify(x => x
        .Convert(It.IsAny<Invoice>(), It.IsAny<string>()), Times.Never());
      Assert.AreEqual(FakeInvoice1, result.Value);
    }

    [Test]
    public async Task GetInvoiceMethodConvertsTheCurrency()
    {
      var result = await _controller.GetInvoice(FakeInvoice1.InvoiceId, "USD");

      _exchangeServiceMock.Verify(x => x.Convert(FakeInvoice1, "USD"), Times.Once());
      Assert.AreEqual(FakeInvoice1, result.Value);
    }

    [Test]
    public void PutInvoiceMethodUpdatesTheInvoice()
    {
      var result = _controller.PutInvoice(FakeInvoice1.InvoiceId, FakeInvoice1);

      _genericRepositoryMock.Verify(x => x.Update(FakeInvoice1), Times.Once());
      _unitOfWorkMock.Verify(x => x.Commit(), Times.Once());
      Assert.AreEqual(FakeInvoice1, result.Value);
    }

    [Test]
    public async Task PostInvoiceMethodRegistersTheInvoice()
    {
      var result = await _controller.PostInvoice(FakeInvoice1);

      _genericRepositoryMock.Verify(x => x.CreateAsync(FakeInvoice1), Times.Once());
      _unitOfWorkMock.Verify(x => x.Commit(), Times.Once());
      Assert.AreEqual(FakeInvoice1, result.Value);
    }

    [Test]
    public async Task DeleteInvoiceMethodRemovesTheInvoice()
    {
      var result = await _controller.DeleteInvoice(FakeInvoice1.InvoiceId);

      _genericRepositoryMock.Verify(x => x.Remove(FakeInvoice1), Times.Once());
      _unitOfWorkMock.Verify(x => x.Commit(), Times.Once());
      Assert.AreEqual(FakeInvoice1, result.Value);
    }

    #endregion

    #region Private methods

    private Mock<IGenericRepository<Invoice>> InstallGenericRepositoryMock()
    {
      var mock = new Mock<IGenericRepository<Invoice>>();
      mock.Setup(x => x.GetAsync())
        .ReturnsAsync(InvoiceList);
      mock.Setup(x => x.GetAsync(It.IsAny<Expression<Func<Invoice, bool>>>(), null))
        .ReturnsAsync(new List<Invoice>() { FakeInvoice1 });
      mock.Setup(x => x.CreateAsync(FakeInvoice1))
        .ReturnsAsync(FakeInvoice1);
      mock.Setup(x => x.Update(FakeInvoice1))
        .Returns(FakeInvoice1);
      mock.Setup(x => x.Remove(FakeInvoice1))
        .Returns(FakeInvoice1);

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

    #endregion
  }
}
