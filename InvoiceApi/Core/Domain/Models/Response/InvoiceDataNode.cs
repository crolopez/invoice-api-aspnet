namespace InvoiceApi.Core.Domain.Models.Response
{
  public class InvoiceDataNode: DataNode<Invoice>
  {
    private const string DataType = "invoice";

    public InvoiceDataNode(Invoice invoice):
      base(invoice.InvoiceId, DataType, invoice)
    {
    }

    public InvoiceDataNode(string invoiceId):
      base(invoiceId, DataType)
    {
    }
  }
}