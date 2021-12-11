namespace InvoiceApi.Core.Domain.Models.Response
{
  public class ErrorNode
  {
    public string Id { get; private set; }
    public string Detail { get; private set; }

    public ErrorNode(string id, string detail)
    {
      this.Id = id;
      this.Detail = detail;
    }
  }
}