using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InvoiceApi.Core.Application.Contracts;
using InvoiceApi.Core.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace InvoiceApi.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoiceController : ControllerBase
    {
        private readonly IGenericRepository<Invoice> _genericRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IExchangeService _exchangeService;
        private readonly IErrorActionFactory _errorActionFactory;

        public InvoiceController(
            IGenericRepository<Invoice> genericRepository,
            IUnitOfWork unitOfWork,
            IExchangeService exchangeService,
            IErrorActionFactory errorActionFactory)
        {
            _genericRepository = genericRepository;
            _unitOfWork = unitOfWork;
            _exchangeService = exchangeService;
            _errorActionFactory = errorActionFactory;
        }

        // GET: api/Invoice
        [HttpGet]
        #nullable enable
        public async Task<ActionResult<InvoiceAction>> GetInvoices([FromQuery] string? currency)
        {
            try
            {
                var invoices = await _genericRepository.GetAsync();

                if (currency != null)
                {
                    if (!_exchangeService.IsValidCurrency(currency))
                    {
                        return GetInvalidCurrencyAction(string.Empty, currency);
                    }

                    foreach (var i in invoices)
                    {
                        _exchangeService.Convert(i, currency);
                    }
                }

                return new InvoiceAction(invoices);
            }
            catch (Exception exception)
            {
                return _errorActionFactory.GetErrorAction(string.Empty, exception);
            }
        }
        #nullable disable

        // GET: api/Invoice/5
        [HttpGet("{id}")]
        #nullable enable
        public async Task<ActionResult<InvoiceAction>> GetInvoice(string id, [FromQuery] string? currency)
        {
            try
            {
                Invoice invoice = await GetInvoice(id);
                if (invoice == null)
                {
                    return GetNotFoundAction(id);
                }

                if (currency != null)
                {
                    if (!_exchangeService.IsValidCurrency(currency))
                    {
                        return GetInvalidCurrencyAction(id, currency);
                    }

                    _exchangeService.Convert(invoice, currency);
                }

                return new InvoiceAction(invoice);
            }
            catch (Exception exception)
            {
                return _errorActionFactory.GetErrorAction(id, exception);
            }
        }
        #nullable disable

        // PUT: api/Invoice/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<ActionResult<InvoiceAction>> PutInvoice(string id, Invoice invoice)
        {
            try
            {
                if (id != invoice.InvoiceId)
                {
                    return _errorActionFactory.GetErrorAction(id, "Request ID doesn't match invoice ID.");
                }

                if (await GetInvoice(id) == null)
                {
                    return GetNotFoundAction(id);
                }

                if (invoice.Currency == string.Empty ||
                    !_exchangeService.IsValidCurrency(invoice.Currency))
                {
                    return GetInvalidCurrencyAction(id, invoice.Currency);
                }

                Invoice updatedInvoice = await _genericRepository.Update(invoice);
                if (updatedInvoice == null ||
                    (await _unitOfWork.Commit()) < 1)
                {
                    return _errorActionFactory.GetErrorAction(id, "The invoice couldn't be updated.");
                }

                return new InvoiceAction(updatedInvoice);
            }
            catch (Exception exception)
            {
                return _errorActionFactory.GetErrorAction(id, exception);
            }
        }

        // POST: api/Invoice
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<InvoiceAction>> PostInvoice(Invoice invoice)
        {
            try
            {
                if (invoice.InvoiceId == string.Empty)
                {
                    return GetEmptyIdAction();
                }

                if (invoice.Currency == string.Empty ||
                    !_exchangeService.IsValidCurrency(invoice.Currency))
                {
                    return GetInvalidCurrencyAction(invoice.InvoiceId, invoice.Currency);
                }

                Invoice savedInvoice = await _genericRepository.CreateAsync(invoice);

                if (savedInvoice != null)
                    await _unitOfWork.Commit();

                return new InvoiceAction(savedInvoice);
            }
            catch (Exception exception)
            {
                return _errorActionFactory.GetErrorAction(invoice.InvoiceId, exception);
            }
        }

        // DELETE: api/Invoice/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<InvoiceAction>> DeleteInvoice(string id)
        {
            try
            {
                Invoice invoice = await GetInvoice(id);
                if (invoice == null)
                {
                    return GetNotFoundAction(id);
                }

                invoice = await _genericRepository.Remove(invoice);

                if (invoice != null)
                    await _unitOfWork.Commit();

                return new InvoiceAction(invoice);
            }
            catch (Exception exception)
            {
                return _errorActionFactory.GetErrorAction(id, exception);
            }
        }

        private InvoiceAction GetNotFoundAction(string id)
        {
            return _errorActionFactory.GetErrorAction(id, "Invoice not found.");
        }

        private InvoiceAction GetEmptyIdAction()
        {
            return _errorActionFactory.GetErrorAction(string.Empty, "Invoice ID cannot be empty.");
        }

        private InvoiceAction GetInvalidCurrencyAction(string id, string currency)
        {
            return currency == string.Empty
                ? _errorActionFactory.GetErrorAction(id, "Currency cannot be empty.")
                : _errorActionFactory.GetErrorAction(id, $"Invalid currency: {currency}.");
        }

        private async Task<Invoice> GetInvoice(string id)
        {
            IEnumerable<Invoice> invoiceList = await _genericRepository.GetAsync(
                    x => x.InvoiceId == id,
                    x => x.OrderBy(x => x.InvoiceId));

            return invoiceList.FirstOrDefault();
        }
    }
}
