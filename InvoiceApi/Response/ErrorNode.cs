namespace InvoiceApi.Response
{
  public class ErrorNode
  {
    public string id { get; private set; }
    public string detail { get; private set; }

    public ErrorNode(string id, string detail) {
      this.id = id;
      this.detail = detail;
    }
  }
}