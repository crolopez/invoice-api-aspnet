using System.Collections.Generic;

namespace InvoiceApi.Core.Domain.Models
{
    public class InvoiceAction
    {
        public IEnumerable<Invoice> InvoiceList { get; set; }
        public int Status { get; set; }
        public string Error { get; set; }

        public InvoiceAction(Invoice invoice)
        {
          Status = 0;
          Error = string.Empty;
          InvoiceList = new List<Invoice>() { invoice };
        }

        public InvoiceAction(IEnumerable<Invoice> invoices)
        {
          Status = 0;
          Error = string.Empty;
          InvoiceList = invoices;
        }

        public InvoiceAction(Invoice invoice, string error)
        {
          Status = 0;
          Error = error;
          InvoiceList = new List<Invoice>() { invoice };
        }
    }
}