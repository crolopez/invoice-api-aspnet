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

        public InvoiceController(
            IGenericRepository<Invoice> genericRepository,
            IUnitOfWork unitOfWork,
            IExchangeService exchangeService)
        {
            _genericRepository = genericRepository;
            _unitOfWork = unitOfWork;
            _exchangeService = exchangeService;
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
                return GetErrorAction(string.Empty, exception.Message);
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
                return GetErrorAction(id, exception.Message);
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
                    return GetErrorAction(id, "Request ID doesn't match invoice ID.");
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
                    return GetErrorAction(id, "The invoice couldn't be updated.");
                }

                return new InvoiceAction(updatedInvoice);
            }
            catch (Exception exception)
            {
                return GetErrorAction(id, exception.Message);
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
                return GetErrorAction(invoice.InvoiceId, exception.Message);
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
                return GetErrorAction(id, exception.Message);
            }
        }

        private InvoiceAction GetNotFoundAction(string id)
        {
            return GetErrorAction(id, "Invoice not found.");
        }

        private InvoiceAction GetEmptyIdAction()
        {
            return GetErrorAction(string.Empty, "Invoice ID cannot be empty.");
        }

        private InvoiceAction GetInvalidCurrencyAction(string id, string currency)
        {
            return currency == string.Empty
                ? GetErrorAction(id, "Currency cannot be empty.")
                : GetErrorAction(id, $"Invalid currency: {currency}.");
        }

        private InvoiceAction GetErrorAction(string id, string error)
        {
            var errorInvoice = new Invoice()
            {
                InvoiceId = id
            };

            return new InvoiceAction(errorInvoice, error);
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
