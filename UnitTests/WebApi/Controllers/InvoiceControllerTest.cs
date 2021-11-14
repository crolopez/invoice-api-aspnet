using System;
using NUnit.Framework;
using Moq;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using InvoiceApi.Core.Domain.Models;
using InvoiceApi.Core.Application.Contracts;
using InvoiceApi.WebApi.Controllers;

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

        private Mock<IInvoiceContext> _mock;
        private InvoiceController _controller;

        #endregion

        #region Test Methods

        [SetUp]
        protected void Setup()
        {
            _mock = new Mock<IInvoiceContext>();
            _mock.Setup(x => x.GetAllInvoices())
                .ReturnsAsync(InvoiceList);
            _mock.Setup(x => x.FindInvoice(FakeInvoice1.InvoiceId))
                .ReturnsAsync(FakeInvoice1);
            _mock.Setup(x => x.UpdateInvoice(FakeInvoice1))
                .Returns(Task.FromResult(FakeInvoice1));
            _mock.Setup(x => x.AddInvoice(FakeInvoice1))
                .Returns(Task.FromResult(FakeInvoice1));
            _mock.Setup(x => x.DeleteInvoice(FakeInvoice1))
                .Returns(Task.FromResult(FakeInvoice1));

            _controller = new InvoiceController(_mock.Object);
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

            Assert.AreEqual(FakeInvoice1, result.Value);
        }

        [Test]
        public async Task PutInvoiceMethodUpdatesTheInvoice()
        {
            var result = await _controller.PutInvoice(FakeInvoice1.InvoiceId, FakeInvoice1);

            _mock.Verify(x => x.UpdateInvoice(FakeInvoice1), Times.Once());
            Assert.AreEqual(FakeInvoice1, result.Value);
        }

        [Test]
        public async Task PostInvoiceMethodRegistersTheInvoice()
        {
            var result = await _controller.PostInvoice(FakeInvoice1);

            _mock.Verify(x => x.AddInvoice(FakeInvoice1), Times.Once());
        }

        [Test]
        public async Task DeleteInvoiceMethodRemovesTheInvoice()
        {
            var result = await _controller.DeleteInvoice(FakeInvoice1.InvoiceId);

            _mock.Verify(x => x.DeleteInvoice(FakeInvoice1), Times.Once());
            Assert.AreEqual(FakeInvoice1, result.Value);
        }

        #endregion
    }
}
