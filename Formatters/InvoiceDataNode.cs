using InvoiceApi.Models;

namespace InvoiceApi.Formatters
{
    public class InvoiceDataNode: DataNode<Invoice>
    {
        public InvoiceDataNode(int id, Invoice invoice):
          base(id, "invoice", invoice)
        {
        }
    }
}