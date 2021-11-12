using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InvoiceApi.Domain.Contracts;
using InvoiceApi.Domain.Models;

namespace InvoiceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoiceController : ControllerBase
    {
        private readonly IInvoiceContext _context;

        public InvoiceController(IInvoiceContext context)
        {
            _context = context;
        }

        // GET: api/Invoice
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Invoice>>> GetInvoices()
        {
            return await _context.GetAllInvoices();
        }

        // GET: api/Invoice/5
        [HttpGet("{id}")]
        #nullable enable
        public async Task<ActionResult<Invoice>> GetInvoice(string id, [FromQuery] string? currency)
        {
            var invoice = currency != null
                ? await _context.FindInvoice(id, currency)
                : await _context.FindInvoice(id);

            if (invoice == null)
            {
                return NotFound();
            }

            return invoice;
        }
        #nullable disable

        // PUT: api/Invoice/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<ActionResult<Invoice>> PutInvoice(string id, Invoice invoice)
        {
            if (id != invoice.InvoiceId)
            {
                return BadRequest();
            }

            try
            {
                await _context.UpdateInvoice(invoice);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (await _context.FindInvoice(id) == null)
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return invoice;
        }

        // POST: api/Invoice
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Invoice>> PostInvoice(Invoice invoice)
        {
            await _context.AddInvoice(invoice);

            return CreatedAtAction(nameof(GetInvoice), new { id = invoice.InvoiceId }, invoice);
        }

        // DELETE: api/Invoice/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Invoice>> DeleteInvoice(string id)
        {
            var invoice = await _context.FindInvoice(id);
            if (invoice == null)
            {
                return NotFound();
            }

            await _context.DeleteInvoice(invoice);

            return invoice;
        }
    }
}
