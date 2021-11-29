using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InvoiceApi.Core.Application.Contracts;
using InvoiceApi.Core.Domain.Models;

namespace InvoiceApi.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoiceController : ControllerBase
    {
        private readonly IGenericRepository<Invoice> _genericRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IExchangeService _exchangeService;

        public InvoiceController(IGenericRepository<Invoice> genericRepository,
            IUnitOfWork unitOfWork, IExchangeService exchangeService)
        {
            _genericRepository = genericRepository;
            _unitOfWork = unitOfWork;
            _exchangeService = exchangeService;
        }

        // GET: api/Invoice
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Invoice>>> GetInvoices()
        {
            var invoices = await _genericRepository.GetAsync();

            return invoices.ToList();
        }

        // GET: api/Invoice/5
        [HttpGet("{id}")]
        #nullable enable
        public async Task<ActionResult<Invoice>> GetInvoice(string id, [FromQuery] string? currency)
        {
            Invoice? invoice = (await _genericRepository
                .GetAsync(x => x.invoiceId == id))?.First();

            if (invoice == null)
            {
                return NotFound();
            }

            if (currency != null)
            {
                _exchangeService.Convert(invoice, currency);
            }

            return invoice;
        }
        #nullable disable

        // PUT: api/Invoice/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public ActionResult<Invoice> PutInvoice(string id, Invoice invoice)
        {
            if (id != invoice.invoiceId)
            {
                return BadRequest();
            }

            Invoice updatedInvoice = _genericRepository.Update(invoice);
            if (updatedInvoice != null) {
                _unitOfWork.Commit();
            }

            return updatedInvoice;
        }

        // POST: api/Invoice
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Invoice>> PostInvoice(Invoice invoice)
        {
            Invoice savedInvoice = await _genericRepository.CreateAsync(invoice);

            if(savedInvoice != null)
                _unitOfWork.Commit();

            return savedInvoice;
        }

        // DELETE: api/Invoice/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Invoice>> DeleteInvoice(string id)
        {
            Invoice invoice = (await GetInvoice(id, null)).Value;
            if (invoice == null)
            {
                return NotFound();
            }

            invoice = _genericRepository.Remove(invoice);

            if(invoice != null)
                _unitOfWork.Commit();

            return invoice;
        }
    }
}
