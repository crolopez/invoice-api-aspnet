namespace InvoiceApi.Core.Domain.Models.Response
{
  public class DataNode<T>
  {
    public string Id { get; private set; }
    public string Type { get; private set; }
    #nullable enable
    public T? Attributes { get; private set; }

    public DataNode(string id, string type, T attributes)
    {
      this.Id = id;
      this.Type = type;
      this.Attributes = attributes;
    }

    public DataNode(string id, string type)
    {
      this.Id = id;
      this.Type = type;
    }
  }
}